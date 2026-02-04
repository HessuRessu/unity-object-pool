using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{

    /// <summary>
    /// Poolable wrapper for a generic <see cref="GameObject"/>.
    /// </summary>
    public class PoolableGameObject : BasePoolable
    {
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