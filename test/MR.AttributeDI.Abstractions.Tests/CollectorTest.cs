using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MR.AttributeDI
{
	[AddToServices]
	public class Service
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
				new Collector(null);
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
			var applier = new FakeApplier();
			var collector = Create();
			collector.Collect(applier);

			applier.Contexts.Should().Contain((c) => c.Service == typeof(Service));
		}

		private Collector Create() => new Collector(typeof(CollectorTest).Assembly);
	}
}
