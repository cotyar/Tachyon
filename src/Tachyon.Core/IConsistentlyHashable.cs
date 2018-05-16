using JetBrains.Annotations;

namespace Tachyon.Core
{
    public interface IConsistentlyHashable
    {
        [Pure]
        int GetConsistentHash();
    }

    public interface IConsistentHasher<in T>
    {
        [Pure]
        int ConsistentHash(T value);
    }
}