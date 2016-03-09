using System;

namespace MR.AttributeDI
{
	/// <summary>
	/// Decorate types with this for the <see cref="Collector"/> to see it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class AddToServicesAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		public AddToServicesAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		/// <param name="lifetime">The lifetime to use.</param>
		/// <param name="as">The type to apply as.</param>
		public AddToServicesAttribute(Lifetime lifetime, Type @as = null)
		{
			Lifetime = lifetime;
			As = @as;
		}

		/// <summary>
		/// Gets or sets the lifetime to use.
		/// </summary>
		public Lifetime Lifetime { get; set; } = Lifetime.Scoped;

		/// <summary>
		/// Gets or sets type to apply as.
		/// </summary>
		public Type As { get; set; }

		/// <summary>
		/// Gets or sets the tag to collect.
		/// </summary>
		public string Tag { get; set; }
	}
}
