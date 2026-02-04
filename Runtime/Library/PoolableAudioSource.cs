using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling.Library
{
    /// <summary>
    /// Poolable wrapper for an <see cref="AudioSource"/>.
    /// </summary>
    public class PoolableAudioSource : BasePoolable
    {
        /// <summary>
        /// AudioSource controlled by this poolable.
        /// </summary>
        public AudioSource audioSource;

        /// <inheritdoc/>
        public override void OnBorrowed()
        {
            this.gameObject.SetActive(true);

            if (this.audioSource != null)
                this.audioSource.Play();
        }

        /// <inheritdoc/>
        public override void OnReturned()
        {
            if (this.audioSource != null)
                this.audioSource.Stop();

            this.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        public override void OnUpdate(float deltaTime) { }
    }
}