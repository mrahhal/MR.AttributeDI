using System;
using Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection;

/// <summary>
/// An applier for Microsoft's <see cref="IServiceCollection"/>.
/// </summary>
public class ServiceCollectionApplier : IApplier
{
	private readonly IServiceCollection _services;

	public ServiceCollectionApplier(IServiceCollection services)
	{
		_services = services ?? throw new ArgumentNullException(nameof(services));
	}

	public void Apply(ApplierContext context)
	{
		if (context.ForwardTo == null)
		{
			_services.Add(new ServiceDescriptor(
				context.Service,
				context.Implementation,
				Convert(context.Lifetime)));
		}
		else
		{
			_services.Add(new ServiceDescriptor(
				context.Service,
				provider => provider.GetService(context.ForwardTo),
				Convert(context.Lifetime)));
		}
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
