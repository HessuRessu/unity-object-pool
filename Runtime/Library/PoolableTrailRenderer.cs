using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="TrailRenderer"/>.
    /// </summary>
    public class PoolableTrailRenderer : BasePoolable
    {
        /// <summary>
        /// Trail renderer component.
        /// </summary>
        public TrailRenderer trailRenderer;

        /// <inheritdoc/>
        public override void OnBorrowed()
        {
            this.gameObject.SetActive(true);
            if (this.trailRenderer != null)
                this.trailRenderer.Clear();
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