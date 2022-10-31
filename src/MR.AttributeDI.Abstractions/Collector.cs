using System.Reflection;

namespace MR.AttributeDI;

/// <summary>
/// Collects types decorated with <see cref="AddToServicesAttribute"/> and presents them to an <see cref="IApplier"/>.
/// </summary>
public class Collector
{
	private readonly IAddToServicesAttributeListProvider _provider;

	/// <summary>
	/// Initializes a new instance of the <see cref="Collector"/> class.
	/// </summary>
	/// <param name="assemblies">The assemblies to collect from.</param>
	/// <exception cref="ArgumentNullException"><paramref name="assemblies"/> is null.</exception>
	/// <exception cref="ArgumentException">Assemblies to check should not be empty.</exception>
	public Collector(params Assembly[] assemblies)
	{
		if (assemblies == null)
			throw new ArgumentNullException(nameof(assemblies));
		if (assemblies.Length == 0)
			throw new ArgumentException("Assemblies to check should not be empty.", nameof(assemblies));

		_provider = new FromAssembliesAddToServicesAttributeListProvider(assemblies);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Collector"/> class.
	/// </summary>
	/// <param name="provider">The provider to collect attributes from.</param>
	/// <exception cref="ArgumentNullException"><paramref name="provider"/> is null.</exception>
	public Collector(IAddToServicesAttributeListProvider provider)
	{
		_provider = provider ?? throw new ArgumentNullException(nameof(provider));
	}

	/// <summary>
	/// Collects types decorated with <see cref="AddToServicesAttribute"/> and user the <paramref name="applier"/> to apply it.
	/// </summary>
	/// <param name="applier">The applier to use.</param>
	/// <param name="tag">The tag to collect.</param>
	/// <exception cref="ArgumentNullException"><paramref name="applier"/> is null.</exception>
	public void Collect(IApplier applier, string tag = null)
	{
		if (applier == null)
			throw new ArgumentNullException(nameof(applier));

		var pairs = _provider.GetAttributes();

		foreach (var pair in pairs)
		{
			var implementation = pair.Implementation;
			var attributes = pair.Attributes;

			foreach (var attribute in attributes)
			{
				if ((attribute.InternalTags == null && tag != null) ||
					(attribute.InternalTags != null && tag == null) ||
					(attribute.InternalTags != null && tag != null && !attribute.InternalTags.Any(
						t => t.Equals(tag, StringComparison.OrdinalIgnoreCase))))
				{
					continue;
				}

				var service = ValidateService(implementation, attribute);
				var context = new ApplierContext(service, implementation, attribute.ForwardTo, attribute.Lifetime);
				applier.Apply(context);
			}
		}
	}

	private Type ValidateService(Type implementation, AddToServicesAttribute attribute)
	{
		if (attribute.As != null)
		{
			return attribute.As;
		}

		if (attribute.AsImplementedInterface)
		{
			return ValidateAsImplementedInterface(implementation);
		}

		return implementation;
	}

	private Type ValidateAsImplementedInterface(Type implementation)
	{
		var interfaces =
			implementation.GetTypeInfo().ImplementedInterfaces as ICollection<Type> ??
			implementation.GetTypeInfo().ImplementedInterfaces.ToList();

		if (interfaces.Count == 0)
		{
			throw new InvalidOperationException($"Type {implementation.FullName} does not implement interfaces.");
		}
		else if (interfaces.Count > 1)
		{
			throw new InvalidOperationException($"Type {implementation.FullName} implements multiple interfaces.");
		}

		return interfaces.First();
	}
}
