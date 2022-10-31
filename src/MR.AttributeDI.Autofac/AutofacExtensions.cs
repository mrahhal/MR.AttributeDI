using System.Reflection;
using Autofac;

namespace MR.AttributeDI.Autofac;

public static class AutofacExtensions
{
	/// <summary>
	/// Configures <see cref="ContainerBuilder"/> using <see cref="AddToServicesAttribute"/> decorated types.
	/// </summary>
	/// <param name="builder">The container builder to configure.</param>
	/// <param name="assemblies">The assemblies to collect the types from.</param>
	public static void ConfigureFromAttributes(this ContainerBuilder builder, params Assembly[] assemblies)
	{
		builder.ConfigureFromAttributes(null, assemblies);
	}

	[Obsolete("Use ConfigureFromAttributes instead.")]
	public static void Configure(this ContainerBuilder builder, params Assembly[] assemblies)
		=> builder.ConfigureFromAttributes(assemblies);

	/// <summary>
	/// Configures <see cref="ContainerBuilder"/> using <see cref="AddToServicesAttribute"/> decorated types.
	/// </summary>
	/// <param name="builder">The container builder to configure.</param>
	/// <param name="tag">The tag to collect.</param>
	/// <param name="assemblies">The assemblies to collect the types from.</param>
	public static void ConfigureFromAttributes(this ContainerBuilder builder, string? tag, params Assembly[] assemblies)
	{
		var collector = new Collector(assemblies);
		var applier = new AutofacApplier(builder);
		collector.Collect(applier, tag);
	}

	[Obsolete("Use ConfigureFromAttributes instead.")]
	public static void Configure(this ContainerBuilder builder, string tag, params Assembly[] assemblies)
		=> builder.ConfigureFromAttributes(tag, assemblies);
}
