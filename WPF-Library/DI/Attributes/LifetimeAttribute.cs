using System;

namespace WPFLibrary.DI.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class LifetimeAttribute : Attribute
{
    public LifetimeAttribute (Lifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public Lifetime Lifetime { get; }
}

public enum Lifetime
{
    Singleton,
    Transient
}