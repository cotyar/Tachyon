using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Tachyon.Core
{
    /// <summary>
    /// Thread-safe random number generator.
    /// Has same API as System.Random but is thread safe, similar to the implementation by Steven Toub: http://blogs.msdn.com/b/pfxteam/archive/2014/10/20/9434171.aspx
    /// </summary>
    static class SafeRandom
    {
        private static readonly RandomNumberGenerator globalCryptoProvider = RandomNumberGenerator.Create();

        [ThreadStatic]
        private static Random current;

        internal static Random Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (current == null)
                {
                    var buffer = new byte[4];
                    globalCryptoProvider.GetBytes(buffer);
                    current = new Random(BitConverter.ToInt32(buffer, 0));
                }

                return current;
            }
        }
    }
}