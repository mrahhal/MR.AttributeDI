using System;
using Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection
{
	public class ServiceCollectionApplier : IApplier
	{
		private IServiceCollection _services;

		public ServiceCollectionApplier(IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			_services = services;
		}

		public void Apply(ApplierContext context)
		{
			_services.Add(new ServiceDescriptor(
				context.As,
				context.Service,
				Convert(context.Lifetime)));
		}

		private ServiceLifetime Convert(Lifetime lifetime)
		{
			switch (lifetime)
			{
				case Lifetime.Singleton:
					return ServiceLifetime.Singleton;
				case Lifetime.Scoped:
					return ServiceLifetime.Scoped;
				case Lifetime.Transient:
				default:
					return ServiceLifetime.Transient;
			}
		}
	}
}
