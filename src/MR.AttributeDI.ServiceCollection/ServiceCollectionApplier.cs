using System;
using Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection
{
	/// <summary>
	/// An applier for Microsoft's <see cref="IServiceCollection"/>.
	/// </summary>
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
				context.Service,
				context.Implementation,
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
