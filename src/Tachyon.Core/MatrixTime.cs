#region copyright
// -----------------------------------------------------------------------
//  <copyright file="MatrixTime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Immutable, Serializable]
    public readonly struct MatrixTime : IConvergent<MatrixTime>
    {
        public bool Equals(MatrixTime other)
        {
            throw new NotImplementedException();
        }

        public MatrixTime Merge(MatrixTime other)
        {
            throw new NotImplementedException();
        }
    }
}