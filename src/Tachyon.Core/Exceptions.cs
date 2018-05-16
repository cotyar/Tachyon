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
        /// <summary>
        /// Checks if provided argument is not null. Otherwise it will throw an
        /// <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throw when <paramref name="value"/> is null.
        /// </exception>
        public static void NotNull<T>(T value) where T: class
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException();
        }
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
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void InvalidOperationException(string msg)
        {
            throw new InvalidOperationException(msg);
        }
    }
}