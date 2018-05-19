#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IFunc.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Core
{
    /// <summary>
    /// An interface used with semantics similar to delegates, but optimized to make use
    /// of struct-based rather than class-based higher order functions.
    /// </summary>
    /// <typeparam name="I">Type of an input.</typeparam>
    /// <typeparam name="O">Type of an output.</typeparam>
    interface IFunc<in I, out O>
    {
        O Invoke(I arg);
    }
}