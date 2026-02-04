using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="LineRenderer"/> component.
    /// </summary>
    public class PoolableLineRenderer : BasePoolable
    {
        /// <summary>
        /// LineRenderer component controlled by this poolable.
        /// </summary>
        public LineRenderer lineRenderer;

        /// <inheritdoc/>
        public override void OnBorrowed()
        {
            this.SetColorKeys(this.Context.color);
            this.gameObject.SetActive(true);
        }

        /// <inheritdoc/>
        public override void OnReturned()
        {
            this.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        public override void OnUpdate(float deltaTime) { }


        public virtual void SetColorKeys(Color color)
        {
            Gradient gradient = this.lineRenderer.colorGradient;

            var keys = gradient.colorKeys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].color = color;
            }

            gradient.colorKeys = keys;
            this.lineRenderer.colorGradient = gradient;
            this.lineRenderer.material.SetColor("_Color", color);
        }
    }
}
