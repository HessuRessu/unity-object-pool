using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for a <see cref="ParticleSystem"/>.
    /// </summary>
    public class PoolableParticleSystem : BasePoolable
    {
        /// <summary>
        /// Particle system controlled by this poolable.
        /// </summary>
        public ParticleSystem particleSys;

        /// <inheritdoc/>
        public override void OnBorrowed()
        {
            this.gameObject.SetActive(true);
            if (this.particleSys != null)
            {
                this.particleSys.Clear(true);
                this.particleSys.Play(true);
            }
        }

        /// <inheritdoc/>
        public override void OnReturned()
        {
            if (this.particleSys != null)
                this.particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            this.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        public override void OnUpdate(float deltaTime) { }
    }
}