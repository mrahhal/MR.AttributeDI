using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MR.AttributeDI;

[AddToServices]
public class Service1 { }

public interface IService2 { }

[AddToServices]
[AddToServices(As = typeof(IService2))]
public class Service2 : IService2 { }

[AddToServices(Tags = "foo")]
public class Service3 { }

[AddToServices(Tags = "foo, bar")]
public class Service4 { }

public interface IServiceAsImplementedInterface1 { }

public interface IServiceAsImplementedInterface2 { }

public interface IServiceAsImplementedInterfaceSub { }

[AddToServices(AsImplementedInterface = true)]
public class ServiceAsImplementedInterfaceOne : IServiceAsImplementedInterface1 { }

[AddToServices(AsImplementedInterface = true)]
public class ServiceAsImplementedInterfaceNone { }

[AddToServices(AsImplementedInterface = true)]
public class ServiceAsImplementedInterfaceMultiple : IServiceAsImplementedInterface1, IServiceAsImplementedInterface2 { }

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
			Create(typeof(Service1)).Collect(null);
		});
	}

	[Fact]
	public void Collect()
	{
		// Assert
		var applier = new FakeApplier();
		var collector = Create(typeof(Service1));

		// Act
		collector.Collect(applier);

		// Assert
		applier.Contexts.Should().Contain(c => c.Implementation == typeof(Service1));
	}

	[Fact]
	public void Collect_Multiple()
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(Service2));

		// Act
		collector.Collect(applier);

		// Assert
		applier.Contexts.Should().Contain(c =>
			c.Implementation == typeof(Service2) && c.Service == typeof(Service2)).And.Contain((c) =>
			c.Implementation == typeof(Service2) && c.Service == typeof(IService2));
	}

	[Fact]
	public void Collect_MultipleTags()
	{
		// Arrange
		var applier1 = new FakeApplier();
		var applier2 = new FakeApplier();
		var collector1 = Create(typeof(Service4));
		var collector2 = Create(typeof(Service4));

		// Act
		collector1.Collect(applier1, "foo");
		collector2.Collect(applier2, "bar");

		// Assert
		applier1.Contexts.Should().Contain(c => c.Implementation == typeof(Service4));
		applier2.Contexts.Should().Contain(c => c.Implementation == typeof(Service4));
	}

	[Theory]
	[InlineData(default(string))]
	[InlineData("bar")]
	public void Collect_DoesNotCollectDifferentTags(string tag)
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(Service3));

		// Act
		collector.Collect(applier, tag);

		// Assert
		applier.Contexts.Should().NotContain(c => c.Implementation == typeof(Service3));
	}

	[Theory]
	[InlineData("foo")]
	[InlineData("FoO")]
	public void Collect_CollectsTags(string tag)
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(Service3));

		// Act
		collector.Collect(applier, tag);

		// Assert
		applier.Contexts.Should().Contain(c => c.Implementation == typeof(Service3));
	}

	[Fact]
	public void Collect_AsImplementedInterface_One()
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(ServiceAsImplementedInterfaceOne));

		// Act
		collector.Collect(applier);

		// Assert
		var c = applier.Contexts.First();
		c.Service.Should().Be(typeof(IServiceAsImplementedInterface1));
	}

	[Fact]
	public void Collect_AsImplementedInterface_None()
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(ServiceAsImplementedInterfaceNone));

		// Act + Assert
		Assert.Throws<InvalidOperationException>(() => collector.Collect(applier));
	}

	[Fact]
	public void Collect_AsImplementedInterface_Multiple()
	{
		// Arrange
		var applier = new FakeApplier();
		var collector = Create(typeof(ServiceAsImplementedInterfaceMultiple));

		// Act + Assert
		Assert.Throws<InvalidOperationException>(() => collector.Collect(applier));
	}

	private Collector Create(Type type) => new Collector(new OneTypeProvider(type));
}

public class OneTypeProvider : IAddToServicesAttributeListProvider
{
	private Type _type;

	public OneTypeProvider(Type type)
	{
		_type = type;
	}

	public List<(Type Implementation, IEnumerable<AddToServicesAttribute> Attributes)> GetAttributes()
	{
		return new List<(Type Implementation, IEnumerable<AddToServicesAttribute> Attributes)>
		{
			(_type, _type.GetCustomAttributes<AddToServicesAttribute>())
		};
	}
}
