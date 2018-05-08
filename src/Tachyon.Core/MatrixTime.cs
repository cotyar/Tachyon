#region copyright
// -----------------------------------------------------------------------
//  <copyright file="MatrixTime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.FSharp.Collections;

namespace Tachyon.Core
{
    using NodeId = Int32;

    /// <summary>
    /// Matrix time implementation of recent timestamp matrix RTM.
    /// See: http://haslab.uminho.pt/ashoker/files/opbaseddais14.pdf
    /// </summary>
    [Immutable, Serializable]
    public readonly struct MatrixTime : IConvergent<MatrixTime>, IEnumerable<KeyValuePair<NodeId, VectorTime>>
    {
        public static readonly MatrixTime Zero = new MatrixTime();

        public VectorTime this[NodeId key] => throw new NotImplementedException();

        public MatrixTime SetTime(NodeId key, VectorTime time)
        {
            throw new NotImplementedException();
        }

        public MatrixTime Update(NodeId key, Func<VectorTime, VectorTime> update)
        {
            throw new NotImplementedException();
        }

        public MatrixTime Merge(MatrixTime other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(MatrixTime other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public IEnumerator<KeyValuePair<int, VectorTime>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}