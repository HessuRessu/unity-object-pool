using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="SpriteRenderer"/>.
    /// </summary>
    public class PoolableSpriteRenderer : BasePoolable
    {
        /// <summary>
        /// SpriteRenderer controlled by this poolable.
        /// </summary>
        public SpriteRenderer spriteRenderer;

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