# Fakeneric

Fakeneric is improved but faked compile-time generic constraints! It powered by Roslyn analyzer infrastructure.

## Constraints

1. `Implements` - requires that `<T>` must be derived from class\interface. You can use multile such constraints in a single generic class, your `<T>` must be derived from all of these types.
2. `NotImplements` - requires that `<T>` must NOT be derived from class\interface. You can use multile such constraints in a single generic class, your `<T>` must be NOT derived from all of these types.
3. `HasConstructorWithNoParameter`, `HasConstructorWithParameter` - requires that `<T>` must have a non-private constructor with the appropriate arguments. You can use multile such constraints in a single generic class, all of these constuctors must exists.

## Details

Imagine we want to constraint `<T>` to have a constructor with `(int, short)` signature and to be derived from `interface IMyInterface { }`.

It's impossible to code with regular C# generic constraints:

```C#

//NOPE... HERE IS A COMPILATION ERROR
    public class Base<T>
        where T : IMyInterface, new(int, short)
    {
    }
```

But with Fakenerics it's now possible. Instead of regular syntax, you need to attach an appropriate Fakeneric interfaces. In our case, we have:

```C#

    public class Base<T> :
        Where<T, Implements<IMyInterface>>,
        //Where<T, NotImplements<MyUglyClassIDontWantToRememberAbout>>,
        Where<T, HasConstructorWithParameter<int, short>>
    {
    }

```

Also we (or our users) have the following payload classes:

```C#
    public class Payload1
    {
        public Payload1(int a)
        {
        }
    }

    public class Payload2 : IMyInterface
    {
        public Payload2(int a, short b)
        {
        }
    }

```
so, `class Derived1 : Base<Payload1>` will fails at compilation stage with the messages `Fakeneric constraint violation: Non-private constructor global::ConsoleApp24.Payload1(int, short) does not found.` and `Fakeneric constraint violation: global::ConsoleApp24.Payload1 cannot be casted to global::ConsoleApp24.IMyInterface.`, but `class Derived2 : Base<Payload2>` will works fine.

More advanced scenarios also possible:

```C#
    interface IMyInterface
    {
    }

    public class Base<T1, T2> :
        Where<T1, HasConstructorWithParameter<int, short>>,
        Where<T2, Implements<IEnumerable<char>>>
    {
    }

//HERE IS OK: string can be casted to IEnumerable<char>
    public class Middle<T1> :
        Base<T1, string>,
        Where<T1, Implements<IMyInterface>> //add one more constraint for T1
    {
    }

    public class Payload1
    {
        public Payload1(int a)
        {
        }
    }

    public class Payload2 : IMyInterface
    {
        public Payload2(int a, short b)
        {
        }
    }

//NOPE... COMPILATION ERRORS:
//Fakeneric constraint violation: global::ConsoleApp24.Payload1 cannot be casted to global::ConsoleApp24.IMyInterface.
//Fakeneric constraint violation: Non-private constructor global::ConsoleApp24.Payload1(int, short) does not found.
    public class Derived1 : Middle<Payload1>
    {
    }

//HERE IS OK:
    public class Derived2 : Middle<Payload2>
    {
    }
```

## How to use

You need to install a [Fakeneric](https://www.nuget.org/packages/Fakeneric) Nuget package to your project via `Manage NuGet Packages` menu or via `Install-Package Fakeneric` command. That's all. You can start using Fakenerics now.

## QA

1. Yes, Fakenerics interfaces will be available at runtime via reflection. There is no ways to overcome it. It's an inevitable side effect unless dotnet will support a more advanced generic constraints.
2. Fakeneric will add its small dll to your distribution.
