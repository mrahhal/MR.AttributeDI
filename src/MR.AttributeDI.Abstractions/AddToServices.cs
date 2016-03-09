using System;

namespace MR.AttributeDI
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class AddToServicesAttribute : Attribute
	{
		public AddToServicesAttribute()
		{
		}

		public AddToServicesAttribute(Lifetime lifetime, Type @as = null)
		{
			Lifetime = lifetime;
			As = @as;
		}

		public Lifetime Lifetime { get; set; } = Lifetime.Shared;

		public Type As { get; set; }
	}
}
