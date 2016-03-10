# MR.AttributeDI

[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-attributedi/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-attributedi)
[![NuGet version](https://badge.fury.io/nu/MR.AttributeDI.svg)](https://www.nuget.org/packages/MR.AttributeDI.Abstractions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Provides attribute based configuration of dependency injection services.

## Supported containers

- Microsoft's [`IServiceCollection`](https://github.com/aspnet/DependencyInjection)
- Autofac

## Example using `IServiceCollection`

```diff
"dependencies": {
  ...
+  "MR.AttributeDI.ServiceCollection": "1.0.0"
},
```

```c#
[AddToServices(Lifetime.Transient)] // This will register DefaultMath as self
[AddToServices(Lifetime.Transient, As = typeof(IMath))]
public class DefaultMath : IMath
{
	public int Add(int x, int y)
	{
		return x + y;
	}
}
```

```cs
using MR.AttributeDI.ServiceCollection;

services.Configure(typeof(Program).GetTypeInfo().Assembly); // Assemblies to search in
```

And then simply:

```cs
provider.GetService<IMath>();
provider.GetService<DefaultMath>():
```

## Using tags

```cs
[AddToServices(Lifetime.Transient, Tag = "foo")]
public class DefaultMath
...
```

Here, `DefaultMath` won't be registered unless we specify its tag in configuration:

```cs
services.Configure(typeof(Program).GetTypeInfo().Assembly); // Will not register DefaultMath

services.Configure("foo", typeof(Program).GetTypeInfo().Assembly); // Will register DefaultMath
```
