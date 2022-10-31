# MR.AttributeDI

[![CI](https://github.com/mrahhal/MR.AttributeDI/actions/workflows/ci.yml/badge.svg)](https://github.com/mrahhal/MR.AttributeDI/actions/workflows/ci.yml)
[![NuGet version](https://badge.fury.io/nu/MR.AttributeDI.Abstractions.svg)](https://www.nuget.org/packages/MR.AttributeDI.Abstractions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

Provides attribute based configuration of dependency injection services.

[CHANGELOG](CHANGELOG.md)

## Supported containers

### MR.AttributeDI.ServiceCollection [![NuGet version](https://badge.fury.io/nu/MR.AttributeDI.ServiceCollection.svg)](https://www.nuget.org/packages/MR.AttributeDI.ServiceCollection)
Microsoft's [`IServiceCollection`](https://github.com/aspnet/DependencyInjection)

### MR.AttributeDI.Autofac [![NuGet version](https://badge.fury.io/nu/MR.AttributeDI.Autofac.svg)](https://www.nuget.org/packages/MR.AttributeDI.Autofac)
[Autofac](https://github.com/autofac/Autofac)

## Example using `IServiceCollection`

```cs
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

services.ConfigureFromAttributes(typeof(Program).GetTypeInfo().Assembly); // Assemblies to search in
```

And then simply:

```cs
provider.GetService<IMath>();
provider.GetService<DefaultMath>():
```

## Using tags

```cs
[AddToServices(Lifetime.Transient, Tags = "foo")]
public class DefaultMath
...
```

Here, `DefaultMath` won't be registered unless we specify its tag in configuration:

```cs
services.ConfigureFromAttributes(typeof(Program).GetTypeInfo().Assembly); // Will not register DefaultMath

services.ConfigureFromAttributes("foo", typeof(Program).GetTypeInfo().Assembly); // Will register DefaultMath
```

Multiple tags can be specified separated by a comma (for example `"default, integration"`).

## Forwarding

You can also forward service registrations for services where you want to share a single instance for multiple interfaces:


```cs
[AddToServices(Lifetime.Singleton, As = typeof(IFoo))]
[AddToServices(Lifetime.Singleton, As = typeof(IBar), ForwardTo = typeof(IFoo))]
public class SomeService : IFoo, IBar
{}
```
