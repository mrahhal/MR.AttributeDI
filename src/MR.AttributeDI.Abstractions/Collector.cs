using System;
using System.Linq;
using System.Reflection;

namespace MR.AttributeDI
{
	public class Collector
	{
		public Collector( params Assembly[] assemblies)
		{
			if (assemblies == null)
				throw new ArgumentNullException(nameof(assemblies));
			if (assemblies.Length == 0)
				throw new ArgumentException("Assemblies to check should not be empty.", nameof(assemblies));

			Assemblies = assemblies;
		}

		public Assembly[] Assemblies { get; }

		public void Collect(IApplier applier)
		{
			if (applier == null)
				throw new ArgumentNullException(nameof(applier));

			var implementations = Assemblies
				.SelectMany(a => a.ExportedTypes)
				.Where(t =>
					t.GetTypeInfo()
					 .CustomAttributes
					 .Any(cd => cd.AttributeType == typeof(AddToServicesAttribute)));

			foreach (var implementation in implementations)
			{
				var attributes = implementation.GetTypeInfo().GetCustomAttributes<AddToServicesAttribute>();
				foreach (var attribute in attributes)
				{
					var context = new ApplierContext(implementation, attribute);
					applier.Apply(context);
				}
			}
		}
	}
}
