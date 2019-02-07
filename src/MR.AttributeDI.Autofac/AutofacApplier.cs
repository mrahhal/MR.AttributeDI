using System;
using Autofac;
using Autofac.Builder;

namespace MR.AttributeDI.Autofac
{
	/// <summary>
	/// An applier for Autofac.
	/// </summary>
	public class AutofacApplier : IApplier
	{
		private readonly ContainerBuilder _builder;

		public AutofacApplier(ContainerBuilder builder)
		{
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));
		}

		public void Apply(ApplierContext context)
		{
			if (context.ForwardTo == null)
			{
				var registration = _builder
					.RegisterType(context.Implementation)
					.As(context.Service)
					.ConfigureLifecycle(context.Lifetime, null);
			}
			else
			{
				var registration = RegistrationBuilder.ForDelegate(context.Service, (c, parameters) =>
				{
					return c.Resolve(context.ForwardTo);
				})
				.ConfigureLifecycle(context.Lifetime, null)
				.CreateRegistration();

				_builder.RegisterComponent(registration);
			}
		}
	}
}
