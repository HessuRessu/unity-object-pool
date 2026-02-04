using Pihkura.Pooling.Data;

namespace Pihkura.Pooling.Abstractions
{
    /// <summary>
    /// Interface implemented by all poolable objects.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// True if the object is currently borrowed from the pool.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Initializes this instance with its owning provider.
        /// Called exactly once after instantiation.
        /// </summary>
        void Initialize(PoolProvider provider);

        /// <summary>
        /// Borrows this object from the pool using the provided context.
        /// </summary>
        void Borrow(PoolableContext context);

        /// <summary>
        /// Returns this object back to the pool.
        /// </summary>
        void Return(bool immediate);
    }
}