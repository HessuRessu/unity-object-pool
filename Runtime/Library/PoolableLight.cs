using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="Light"/> component.
    /// </summary>
    public class PoolableLight : BasePoolable
    {
        /// <summary>
        /// Light component.
        /// </summary>
        public Light lightSource;

        /// <inheritdoc/>
        public override void OnBorrowed()
        {
            this.gameObject.SetActive(true);
        }

        /// <inheritdoc/>
        public override void OnReturned()
        {
            this.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        public override void OnUpdate(float deltaTime) { }
    }
}