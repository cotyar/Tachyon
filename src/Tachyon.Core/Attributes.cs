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

}