using System;

namespace MR.AttributeDI
{
	/// <summary>
	/// The context used when applying a service.
	/// </summary>
	public class ApplierContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplierContext"/> class.
		/// </summary>
		/// <param name="service">The service to apply.</param>
		/// <param name="attribute">The attribute that decorates the service.</param>
		public ApplierContext(Type service, AddToServicesAttribute attribute)
		{
			Service = service;
			Lifetime = attribute.Lifetime;
			As = attribute.As ?? service;
		}

		/// <summary>
		/// Gets the service to apply.
		/// </summary>
		public Type Service { get; }

		/// <summary>
		/// Gets the lifetime to use.
		/// </summary>
		public Lifetime Lifetime { get; }

		/// <summary>
		/// Gets the type to apply as.
		/// </summary>
		public Type As { get; }
	}
}
