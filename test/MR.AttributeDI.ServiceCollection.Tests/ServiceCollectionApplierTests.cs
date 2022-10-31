using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MDI = Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection;

[AddToServices]
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

public class ServiceCollectionApplierTests
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
		var services = CreateServices();
		var applier = Create(services);
		var context = new ApplierContext(typeof(Service1), typeof(Service1), Lifetime.Scoped);

		// Act
		applier.Apply(context);

		// Assert
		services.Should().Contain(sd =>
			sd.ServiceType == typeof(Service1) && sd.Lifetime == ServiceLifetime.Scoped);
	}

	[Fact]
	public void Apply_Lifetime()
	{
		// Arrange
		var services = CreateServices();
		var applier = Create(services);
		var context = new ApplierContext(typeof(Service1), typeof(Service1), Lifetime.Transient);

		// Act
		applier.Apply(context);

		// Assert
		services.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Transient);
	}

	[Fact]
	public void Apply_As()
	{
		// Arrange
		var services = CreateServices();
		var applier = Create(services);
		var context = new ApplierContext(typeof(IService1), typeof(Service1), Lifetime.Scoped);

		// Act
		applier.Apply(context);

		// Assert
		services.Should().Contain(sd =>
			sd.ServiceType == typeof(IService1) &&
			sd.ImplementationType == typeof(Service1));
	}

	[Fact]
	public void Apply_Multiple()
	{
		// Arrange
		var services = CreateServices();
		var applier = Create(services);
		var context = new ApplierContext(typeof(IService1), typeof(Service1), Lifetime.Scoped);

		// Act
		applier.Apply(context);

		// Assert
		services.Should().Contain(sd =>
			sd.ServiceType == typeof(IService1) &&
			sd.ImplementationType == typeof(Service1));
	}

	[Fact]
	public void Apply_FowardTo()
	{
		// Arrange
		var services = CreateServices();
		var applier = Create(services);
		var context1 = new ApplierContext(typeof(IMultipleImplementedInterfaces1), typeof(MultipleImplementedInterfaces), null, Lifetime.Scoped);
		var context2 = new ApplierContext(typeof(IMultipleImplementedInterfaces2), typeof(MultipleImplementedInterfaces), typeof(IMultipleImplementedInterfaces1), Lifetime.Scoped);

		// Act
		applier.Apply(context1);
		applier.Apply(context2);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var _1 = serviceProvider.GetService<IMultipleImplementedInterfaces1>();
		var _2 = serviceProvider.GetService<IMultipleImplementedInterfaces2>();

		_1.Should().BeSameAs(_2);
	}

	[Fact]
	public void ServiceCollectionExtensions()
	{
		// Arrange
		var services = CreateServices();

		// Act
		services.ConfigureFromAttributes(typeof(ServiceCollectionApplierTests).Assembly);

		// Assert
		services.Should().NotBeEmpty();
	}

	private ServiceCollectionApplier Create(IServiceCollection services)
		=> new ServiceCollectionApplier(services);

	private MDI.ServiceCollection CreateServices() => new MDI.ServiceCollection();
}
