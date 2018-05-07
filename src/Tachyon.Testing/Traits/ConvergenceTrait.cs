using Tachyon.Core;

namespace Tachyon.Testing.Traits
{
    /// <summary>
    /// A set of facts to be implemented by a <see cref="IConvergent{T}"/> implementation
    /// in order to check it's satisfying major convergence properties:
    ///
    /// 1. Associativity: x • (y • z) = (x • y) • z
    /// 2. Commutativity: x • y = y • x
    /// 3. Idempotency: x • x = x
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ConvergenceChecks 
    {
        /// <summary>
        /// Checks if associativity rules (x • (y • z) = (x • y) • z) for instances of type
        /// <typeparamref name="T"/> are satisfied in context of
        /// <see cref="IConvergent{T}.Merge"/> method.
        /// </summary>
        public static bool SatisfyAssociativity<T>(T x, T y, T z) where T : IConvergent<T>
        {
            var left = x.Merge(y.Merge(z));
            var right = (x.Merge(y)).Merge(z);

            return left.Equals(right);
        }

        /// <summary>
        /// Checks if commutativity rules (x • y = y • x) for instances of type
        /// <typeparamref name="T"/> are satisfied in context of
        /// <see cref="IConvergent{T}.Merge"/> method.
        /// </summary>
        public static bool SatisfyCommutativity<T>(T x, T y) where T : IConvergent<T>
        {
            var left = x.Merge(y);
            var right = y.Merge(x);

            return left.Equals(right);
        }

        /// <summary>
        /// Checks if idempotency rules (x • x = x) for instances of type
        /// <typeparamref name="T"/> are satisfied in context of
        /// <see cref="IConvergent{T}.Merge"/> method.
        /// </summary>
        public static bool SatisfyIdempotency<T>(T zero) where T : IConvergent<T>
        {
            var left = zero.Merge(zero);
            return left.Equals(zero);
        }
    }
}