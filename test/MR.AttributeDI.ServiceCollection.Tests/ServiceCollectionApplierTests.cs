using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MDI = Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection
{
	[AddToServices]
	public class Service1 : IService1
	{
	}

	public interface IService1
	{
	}

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute());

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute(Lifetime.Transient));

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute(Lifetime.Scoped, typeof(IService1)));

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute(Lifetime.Scoped, typeof(IService1)));

			// Act
			applier.Apply(context);

			// Assert
			services.Should().Contain(sd =>
				sd.ServiceType == typeof(IService1) &&
				sd.ImplementationType == typeof(Service1));
		}

		[Fact]
		public void ServiceCollectionExtensions()
		{
			// Arrange
			var services = CreateServices();

			// Act
			services.Configure(typeof(ServiceCollectionApplierTests).Assembly);

			// Assert
			services.Should().NotBeEmpty();
		}

		private ServiceCollectionApplier Create(IServiceCollection services)
			=> new ServiceCollectionApplier(services);

		private MDI.ServiceCollection CreateServices() => new MDI.ServiceCollection();
	}
}
