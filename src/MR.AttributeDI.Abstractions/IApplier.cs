namespace MR.AttributeDI;

/// <summary>
/// Represents an applier that gets fed into a <see cref="Collector"/>.
/// </summary>
public interface IApplier
{
	/// <summary>
	/// Applies the service using the applying context.
	/// </summary>
	/// <param name="context">The applying context.</param>
	void Apply(ApplierContext context);
}
