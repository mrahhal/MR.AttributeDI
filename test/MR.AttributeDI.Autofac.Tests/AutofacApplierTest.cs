using System;
using Autofac;
using FluentAssertions;
using Xunit;

namespace MR.AttributeDI.Autofac
{
	[AddToServices(Lifetime.Transient)]
	public class Service1 : IService1
	{
	}

	public interface IService1
	{
	}

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute(Lifetime.Transient));

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
			var context = new ApplierContext(typeof(Service1), new AddToServicesAttribute(Lifetime.Transient, typeof(IService1)));

			// Act
			applier.Apply(context);

			// Assert
			var container = builder.Build();
			container.Resolve<IService1>().Should().NotBeNull().And.BeOfType<Service1>();
		}

		[Fact]
		public void AutofacExtensions()
		{
			// Arrange
			var builder = CreateBuilder();

			// Act
			builder.Configure(typeof(AutofacApplierTest).Assembly);

			// Assert
			var container = builder.Build();
			container.Resolve<Service1>().Should().NotBeNull();
		}

		private AutofacApplier Create(ContainerBuilder builder) => new AutofacApplier(builder);

		private ContainerBuilder CreateBuilder() => new ContainerBuilder();
	}
}
