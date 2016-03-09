using System.Reflection;
using Autofac;

namespace MR.AttributeDI.Autofac
{
	public static class AutofacExtensions
	{
		public static void Configure(this ContainerBuilder builder, params Assembly[] assemblies)
		{
			var collector = new Collector(assemblies);
			var applier = new AutofacApplier(builder);
			collector.Collect(applier);
		}
	}
}
