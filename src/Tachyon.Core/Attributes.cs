#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Attributes.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core
{
    public abstract class TachyonAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ImmutableAttribute : TachyonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter|AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class NullableAttribute : TachyonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed class PureAttribute : TachyonAttribute
    {
    }
}