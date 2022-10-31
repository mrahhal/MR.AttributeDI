using Autofac;
using FluentAssertions;
using Xunit;

namespace MR.AttributeDI.Autofac;

[AddToServices(Lifetime.Transient)]
public class Service1 : IService1
{
}

public interface IService1
{
}

public class MultipleImplementedInterfaces : IMultipleImplementedInterfaces1, IMultipleImplementedInterfaces2
{
}

public interface IMultipleImplementedInterfaces1 { }

public interface IMultipleImplementedInterfaces2 { }

public class AutofacApplierTest
{
	[Fact]
	public void Ctor_ArgumentNullCheck()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			Create(null);
		});
	}

	[Fact]
	public void Apply_Default()
	{
		// Arrange
		var builder = CreateBuilder();
		var applier = Create(builder);
		var context = new ApplierContext(typeof(Service1), typeof(Service1), Lifetime.Transient);

		// Act
		applier.Apply(context);

		// Assert
		var container = builder.Build();
		container.Resolve<Service1>().Should().NotBeNull();
	}

	[Fact]
	public void Apply_As()
	{
		// Arrange
		var builder = CreateBuilder();
		var applier = Create(builder);
		var context = new ApplierContext(typeof(IService1), typeof(Service1), Lifetime.Transient);

		// Act
		applier.Apply(context);

		// Assert
		var container = builder.Build();
		container.Resolve<IService1>().Should().NotBeNull().And.BeOfType<Service1>();
	}

	[Fact]
	public void Apply_FowardTo()
	{
		// Arrange
		var builder = CreateBuilder();
		var applier = Create(builder);
		var context1 = new ApplierContext(typeof(IMultipleImplementedInterfaces1), typeof(MultipleImplementedInterfaces), null, Lifetime.Scoped);
		var context2 = new ApplierContext(typeof(IMultipleImplementedInterfaces2), typeof(MultipleImplementedInterfaces), typeof(IMultipleImplementedInterfaces1), Lifetime.Scoped);

		// Act
		applier.Apply(context1);
		applier.Apply(context2);

		// Assert
		var container = builder.Build();
		var _1 = container.Resolve<IMultipleImplementedInterfaces1>();
		var _2 = container.Resolve<IMultipleImplementedInterfaces2>();

		_1.Should().BeSameAs(_2);
	}

	[Fact]
	public void AutofacExtensions()
	{
		// Arrange
		var builder = CreateBuilder();

		// Act
		builder.ConfigureFromAttributes(typeof(AutofacApplierTest).Assembly);

		// Assert
		var container = builder.Build();
		container.Resolve<Service1>().Should().NotBeNull();
	}

	private AutofacApplier Create(ContainerBuilder builder) => new AutofacApplier(builder);

	private ContainerBuilder CreateBuilder() => new ContainerBuilder();
}
