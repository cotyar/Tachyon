namespace Tachyon.Core
{
    public interface IConsistentlyHashable
    {
        int GetConsistentHash();
    }

    public interface IConsistentHasher<in T>
    {
        int ConsistentHash(T value);
    }
}