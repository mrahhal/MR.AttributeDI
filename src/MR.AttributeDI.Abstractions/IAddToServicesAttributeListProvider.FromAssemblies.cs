using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MR.AttributeDI
{
	public class FromAssembliesAddToServicesAttributeListProvider : IAddToServicesAttributeListProvider
	{
		private Assembly[] _assemblies;

		public FromAssembliesAddToServicesAttributeListProvider(Assembly[] assemblies)
		{
			_assemblies = assemblies;
		}

		public List<(Type Implementation, IEnumerable<AddToServicesAttribute> Attributes)> GetAttributes()
		{
			var list = new List<(Type, IEnumerable<AddToServicesAttribute>)>();

			var implementations = _assemblies
				.SelectMany(a => a.ExportedTypes)
				.Where(t => t
					.GetTypeInfo()
					.CustomAttributes
					.Any(cd => cd.AttributeType == typeof(AddToServicesAttribute)));

			foreach (var implementation in implementations)
			{
				var attributes = implementation.GetTypeInfo().GetCustomAttributes<AddToServicesAttribute>();
				list.Add((implementation, attributes));
			}

			return list;
		}
	}
}
