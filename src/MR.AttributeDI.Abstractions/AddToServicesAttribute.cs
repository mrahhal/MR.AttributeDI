using System;
using System.Linq;

namespace MR.AttributeDI
{
	/// <summary>
	/// Decorate types with this for the <see cref="Collector"/> to see it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class AddToServicesAttribute : Attribute
	{
		private string _tags;

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		public AddToServicesAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		/// <param name="tags">The tags to use.</param>
		public AddToServicesAttribute(string tags)
			: this(Lifetime.Scoped, null, tags)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		/// <param name="lifetime">The lifetime to use.</param>
		/// <param name="tags">The tags to use.</param>
		public AddToServicesAttribute(Lifetime lifetime, string tags)
			: this(lifetime, null, tags)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToServicesAttribute"/> class.
		/// </summary>
		/// <param name="lifetime">The lifetime to use.</param>
		/// <param name="as">The type to apply as.</param>
		/// <param name="tags">The tags to use.</param>
		public AddToServicesAttribute(Lifetime lifetime, Type @as = null, string tags = null)
		{
			Lifetime = lifetime;
			As = @as;
			Tags = tags;
		}

		/// <summary>
		/// Gets or sets the lifetime to use.
		/// </summary>
		public Lifetime Lifetime { get; set; } = Lifetime.Scoped;

		/// <summary>
		/// Gets or sets the type to apply as.
		/// </summary>
		public Type As { get; set; }

		/// <summary>
		/// Gets or sets whether this should be registered as the implemented interface.
		/// This won't work if the type implements none or multiple interfaces.
		/// </summary>
		public bool AsImplementedInterface { get; set; }

		/// <summary>
		/// Gets or sets the type to forward to.
		/// </summary>
		public Type ForwardTo { get; set; }

		/// <summary>
		/// Gets or sets the tags separated by a comma.
		/// </summary>
		public string Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				_tags = value;
				if (value != null)
				{
					InternalTags = value.Split(',').Select(t => t.Trim()).ToArray();
				}
			}
		}

		[Obsolete("Use the Tags property instead.")]
		public string Tag
		{
			get { return Tags; }
			set { Tags = value; }
		}

		internal string[] InternalTags { get; private set; }
	}
}
