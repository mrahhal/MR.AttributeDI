using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MR.AttributeDI
{
	[AddToServices]
	public class Service1
	{
	}

	public interface IService2
	{
	}

	[AddToServices]
	[AddToServices(As = typeof(IService2))]
	public class Service2 : IService2
	{
	}

	[AddToServices(Tag = "foo")]
	public class Service3
	{
	}

	public class FakeApplier : IApplier
	{
		public void Apply(ApplierContext context)
		{
			Contexts.Add(context);
		}

		public List<ApplierContext> Contexts { get; set; } = new List<ApplierContext>();
	}

	public class CollectorTest
	{
		[Fact]
		public void Ctor_ArgumentNullCheck()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				new Collector(default(Assembly[]));
			});
		}

		[Fact]
		public void Ctor_ArgumentCheck()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				new Collector(new Assembly[0]);
			});
		}

		[Fact]
		public void Collect_ArgumentNullCheck()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Create().Collect(null);
			});
		}

		[Fact]
		public void Collect()
		{
			// Assert
			var applier = new FakeApplier();
			var collector = Create();

			// Act
			collector.Collect(applier);

			// Assert
			applier.Contexts.Should().Contain(c => c.Service == typeof(Service1));
		}

		[Fact]
		public void Collect_Multiple()
		{
			// Arrange
			var applier = new FakeApplier();
			var collector = Create();

			// Act
			collector.Collect(applier);

			// Assert
			applier.Contexts.Should().Contain(c =>
				c.Service == typeof(Service2) && c.As == typeof(Service2)).And.Contain((c) =>
				c.Service == typeof(Service2) && c.As == typeof(IService2));
		}

		[Theory]
		[InlineData(default(string))]
		[InlineData("bar")]
		public void Collect_DoesNotCollectDifferentTags(string tag)
		{
			// Arrange
			var applier = new FakeApplier();
			var collector = Create();

			// Act
			collector.Collect(applier, tag);

			// Assert
			applier.Contexts.Should().NotContain(c => c.Service == typeof(Service3));
		}

		[Theory]
		[InlineData("foo")]
		[InlineData("FoO")]
		public void Collect_CollectsTags(string tag)
		{
			// Arrange
			var applier = new FakeApplier();
			var collector = Create();

			// Act
			collector.Collect(applier, tag);

			// Assert
			applier.Contexts.Should().Contain(c => c.Service == typeof(Service3));
		}

		private Collector Create() => new Collector(typeof(CollectorTest).Assembly);
	}
}
