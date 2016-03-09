using System.Reflection;
using Autofac;

namespace MR.AttributeDI.Autofac
{
	public static class AutofacExtensions
	{
		/// <summary>
		/// Configures <see cref="ContainerBuilder"/> using <see cref="AddToServicesAttribute"/> decorated types.
		/// </summary>
		/// <param name="builder">The container builder to configure.</param>
		/// <param name="assemblies">The assemblies to collect the types from.</param>
		public static void Configure(this ContainerBuilder builder, params Assembly[] assemblies)
		{
			builder.Configure(null, assemblies);
		}

		/// <summary>
		/// Configures <see cref="ContainerBuilder"/> using <see cref="AddToServicesAttribute"/> decorated types.
		/// </summary>
		/// <param name="builder">The container builder to configure.</param>
		/// <param name="tag">The tag to collect.</param>
		/// <param name="assemblies">The assemblies to collect the types from.</param>
		public static void Configure(this ContainerBuilder builder, string tag, params Assembly[] assemblies)
		{
			var collector = new Collector(assemblies);
			var applier = new AutofacApplier(builder);
			collector.Collect(applier, tag);
		}
	}
}
