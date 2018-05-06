#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ByteArrayHelper.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Numerics;

namespace Tachyon.Core
{
    static class ByteArrayHelper
    {
        /// <summary>
        /// Checks if two byte arrays are equal in terms of structural equality of their content.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool StructuralyEquals(byte[] x, byte[] y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x.Length != y.Length) return false;

            if (Vector.IsHardwareAccelerated)
            {
                return VectorEquals(x, y);
            }
            else
            {
                return ByteEquals(x, y, 0);
            }
        }

        private static bool ByteEquals(byte[] x, byte[] y, int i)
        {
            // arrays have been checked for equal length before
            for (; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }

            return true;
        }

        private static bool VectorEquals(byte[] x, byte[] y)
        {
            // arrays have been checked for equal length before
            var size = Vector<byte>.Count;
            var total = x.Length;
            var i = 0;
            for (; i + size < total; i += size)
            {
                var vx = new Vector<byte>(x, i);
                var vy = new Vector<byte>(y, i);

                if (!Vector.EqualsAll(vx, vy)) return false;
            }

            return i == total || ByteEquals(x, y, i);
        }
    }
}