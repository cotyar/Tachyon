#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IConvergent.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Core
{
    /// <summary>
    /// A convergent interface defines a single <see cref="Merge"/> function that is expected to satisfy 3 properties:
    /// 1. Commutativity: (x • y = y • x)    
    /// 2. Associativity: (x • y) • z = z • (y • z)
    /// 3. Idempotency: x • x = x
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConvergent<T>
    {
        T Merge(T other);
    }
}