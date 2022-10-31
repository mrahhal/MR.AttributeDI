namespace MR.AttributeDI;

/// <summary>
/// Represents a service's lifetime that will get applied to an IOC container.
/// </summary>
public enum Lifetime
{
	/// <summary>
	/// A singleton service.
	/// </summary>
	Singleton = 0,

	/// <summary>
	/// A shared service inside a certain scope.
	/// </summary>
	Scoped = 1,

	/// <summary>
	/// A transient service.
	/// </summary>
	Transient = 2
}
