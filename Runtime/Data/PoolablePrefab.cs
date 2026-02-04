using System;
using Pihkura.Pooling.Implementations;

namespace Pihkura.Pooling.Data
{
    /// <summary>
    /// Defines a prefab entry used by <see cref="PoolProvider"/> to create and prewarm pooled objects.
    /// Each entry binds a concrete <see cref="BasePoolable"/> prefab with an initial prewarm count.
    /// </summary>
    [Serializable]
    public struct PoolablePrefab
    {
        /// <summary>
        /// Prefab instance used as the factory source for this poolable type.
        /// </summary>
        public BasePoolable prefab;

        /// <summary>
        /// Number of instances created during provider prewarm.
        /// </summary>
        public int prewarmCount;
    }
}