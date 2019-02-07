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
		public ApplierContext(Type service, Type implementation, Lifetime lifetime)
			: this(service, implementation, null, lifetime)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplierContext"/> class.
		/// </summary>
		public ApplierContext(Type service, Type implementation, Type forwardTo, Lifetime lifetime)
		{
			Service = service;
			Implementation = implementation;
			Lifetime = lifetime;
			ForwardTo = forwardTo;
		}

		/// <summary>
		/// Gets the type to apply as.
		/// </summary>
		public Type Service { get; }

		/// <summary>
		/// Gets the type to apply.
		/// </summary>
		public Type Implementation { get; }

		/// <summary>
		/// Gets the type to forward to.
		/// </summary>
		public Type ForwardTo { get; }

		/// <summary>
		/// Gets the lifetime to use.
		/// </summary>
		public Lifetime Lifetime { get; }
	}
}
