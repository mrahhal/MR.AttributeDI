using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MR.AttributeDI.ServiceCollection
{
	public static class ServiceCollectionExtensions
	{
		public static void Configure(this IServiceCollection services, params Assembly[] assemblies)
		{
			var collector = new Collector(assemblies);
			var applier = new ServiceCollectionApplier(services);
			collector.Collect(applier);
		}
	}
}
