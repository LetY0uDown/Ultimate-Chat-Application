using System;
using System.Diagnostics.CodeAnalysis;

namespace WPFLibrary.Navigation.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ParameterAttribute : Attribute
{
    public ParameterAttribute (Type paramType, string name)
    {
        ParameterType = paramType;
        Name = name;
    }

    public string Name { get; private init; }

    public Type ParameterType { get; private init; }

    public override bool Equals ([NotNullWhen(true)] object? obj)
    {
        if (obj is ParameterAttribute second)
            return Name == second.Name && ParameterType == second.ParameterType;

        return false;
    }

    public override int GetHashCode ()
    {
        return base.GetHashCode();
    }
}
