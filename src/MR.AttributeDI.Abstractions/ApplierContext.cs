using System;

namespace MR.AttributeDI
{
	public class ApplierContext
	{
		public ApplierContext(Type service, AddToServicesAttribute attribute)
		{
			Service = service;
			Lifetime = attribute.Lifetime;
			As = attribute.As ?? service;
		}

		public Type Service { get; set; }

		public Lifetime Lifetime { get; }

		public Type As { get; }
	}
}
