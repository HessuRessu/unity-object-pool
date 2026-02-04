using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="MeshRenderer"/> and <see cref="MeshFilter"/> pair.
    /// </summary>
    public class PoolableMeshRenderer : BasePoolable
    {
        /// <summary>
        /// Mesh renderer.
        /// </summary>
        public MeshRenderer meshRenderer;

        /// <summary>
        /// Mesh filter.
        /// </summary>
        public MeshFilter meshFilter;

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