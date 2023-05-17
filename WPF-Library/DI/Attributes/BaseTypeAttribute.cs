using System;

namespace WPFLibrary.DI.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
public class BaseTypeAttribute : Attribute { }