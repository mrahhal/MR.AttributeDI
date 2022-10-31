using Autofac.Builder;

namespace MR.AttributeDI;

public static class AutoFacRegistrationExtensions
{
	public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
		this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
		Lifetime lifetime,
		object? lifetimeScopeTagForSingleton)
	{
		switch (lifetime)
		{
			case Lifetime.Singleton:
				if (lifetimeScopeTagForSingleton == null)
				{
					registrationBuilder.SingleInstance();
				}
				else
				{
					registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
				}
				break;

			case Lifetime.Scoped:
				registrationBuilder.InstancePerLifetimeScope();
				break;

			case Lifetime.Transient:
				registrationBuilder.InstancePerDependency();
				break;
		}

		return registrationBuilder;
	}
}
