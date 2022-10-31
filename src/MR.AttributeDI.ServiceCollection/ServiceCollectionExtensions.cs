using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Configures <see cref="IServiceCollection"/> using <see cref="AddToServicesAttribute"/> decorated types.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <param name="assemblies">The assemblies to collect the types from.</param>
	public static void ConfigureFromAttributes(this IServiceCollection services, params Assembly[] assemblies)
	{
		services.ConfigureFromAttributes(null, assemblies);
	}

	[Obsolete("Use ConfigureFromAttributes instead.")]
	public static void Configure(this IServiceCollection services, params Assembly[] assemblies)
		=> services.ConfigureFromAttributes(assemblies);

	/// <summary>
	/// Configures <see cref="IServiceCollection"/> using <see cref="AddToServicesAttribute"/> decorated types.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <param name="tag">The tag to collect.</param>
	/// <param name="assemblies">The assemblies to collect the types from.</param>
	public static void ConfigureFromAttributes(this IServiceCollection services, string? tag, params Assembly[] assemblies)
	{
		var collector = new Collector(assemblies);
		var applier = new ServiceCollectionApplier(services);
		collector.Collect(applier, tag);
	}

	[Obsolete("Use ConfigureFromAttributes instead.")]
	public static void Configure(this IServiceCollection services, string? tag, params Assembly[] assemblies)
		=> services.ConfigureFromAttributes(tag, assemblies);
}
