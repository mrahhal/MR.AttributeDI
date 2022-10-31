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
				provider => provider.GetRequiredService(context.ForwardTo),
				Convert(context.Lifetime)));
		}
	}

	private static ServiceLifetime Convert(Lifetime lifetime)
	{
		return lifetime switch
		{
			Lifetime.Singleton => ServiceLifetime.Singleton,
			Lifetime.Scoped => ServiceLifetime.Scoped,
			Lifetime.Transient => ServiceLifetime.Transient,
			_ => throw new Exception("Unreachable"),
		};
	}
}
