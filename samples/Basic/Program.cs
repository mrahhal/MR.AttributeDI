using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MR.AttributeDI.ServiceCollection;

namespace Basic
{
	public class Program
	{
		public static void Main(string[] args)
		{
			PrintUsingServiceCollection();
		}

		private static void PrintUsingServiceCollection()
		{
			var provider = CreateProvider();
			var math = provider.GetRequiredService<IMath>();

			// This would also work because we configured this using AddToServicesAttribute:
			//var math = provider.GetRequiredService<DefaultMath>();

			Print(math, "IServiceCollection");
		}

		private static IServiceProvider CreateProvider()
		{
			var services = new ServiceCollection();

			// Configure the service collection using attributes.
			// Configure is an extension method from MR.AttributeDI.ServiceCollection.
			// GetTypeInfo() is necessary to support .Net Core.
			services.Configure(typeof(Program).GetTypeInfo().Assembly);

			return services.BuildServiceProvider();
		}

		private static void Print(IMath math, string @using)
		{
			Console.WriteLine($"42 + 42 = {math.Add(42, 42)} using {@using}");
		}

	}
}
