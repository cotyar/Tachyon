#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Exceptions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core
{
    /// <summary>
    /// <see cref="Exception"/> helper used to throw exceptions in preconditions.
    /// </summary>
    /// <seealso cref="Raise"/>
    /// <remarks>
    /// Keep in mind that .NET CLR doesn't inline methods with explicit throw
    /// calls in them.
    /// </remarks>
    static class Require
    {

    }

    /// <summary>
    /// <see cref="Exception"/> helper used to throw exceptions where <see cref="Require"/>
    /// is not used.
    /// </summary>
    /// <seealso cref="Require"/>
    /// <remarks>
    /// Keep in mind that .NET CLR doesn't inline methods with explicit throw
    /// calls in them.
    /// </remarks>
    static class Raise
    {

    }
}