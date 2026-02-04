using System;
using Pihkura.Pooling.Abstractions;
using Pihkura.Pooling.Data;
using UnityEngine;

namespace Pihkura.Pooling.Implementations
{
    /// <summary>
    /// Base class for all pooled MonoBehaviours.
    /// Handles lifetime, context storage, and communication with <see cref="PoolProvider"/>.
    /// </summary>
    public abstract class BasePoolable : MonoBehaviour, IPoolable
    {
        private PoolProvider _provider;

        /// <summary>
        /// True if this instance is currently borrowed from the pool.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Elapsed time since Borrow was called.
        /// </summary>
        public float Timer { get; set; }

        /// <summary>
        /// Current borrow context.
        /// </summary>
        public PoolableContext Context { get; private set; }

        private bool _isReturning;

        #region UnityCallbacks

        /// <summary>
        /// Unity update loop. Handles lifetime tracking and calls <see cref="OnUpdate"/>.
        /// </summary>
        void Update()
        {
            if (!this.IsActive)
                return;

            float deltaTime = Time.deltaTime;
            this.Timer += deltaTime;
            this.OnUpdate(deltaTime);

            if (this.Context.lifeTime < 0)
                return;

            if (!this._isReturning && this.Timer >= this.Context.lifeTime)
                this.Return(false);
        }

        #endregion

        /// <summary>
        /// Initializes this instance with its owning provider.
        /// </summary>
        /// <param name="provider">Owning <see cref="PoolProvider"/>.</param>
        public void Initialize(PoolProvider provider)
        {
            this._provider = provider;
        }

        /// <summary>
        /// Called by the provider when this instance is borrowed.
        /// Applies transform data and resets internal state.
        /// </summary>
        /// <param name="context">Borrow context containing spatial data and lifetime.</param>
        public virtual void Borrow(PoolableContext context)
        {
            if (this._provider == null)
                throw new InvalidOperationException("Poolable has not been initialized by PoolProvider.");

            this.Context = context;
            this.transform.position = context.position;
            this.transform.rotation = context.rotation;
            this.transform.localScale = context.scale == default ? Vector3.one : context.scale;

            if (context.parent != null)
                this.transform.SetParent(context.parent, true);
            else if (this.transform.parent != this._provider.poolParent)
                this.transform.SetParent(this._provider.poolParent, true);

            this.Timer = 0f;
            this._isReturning = false;
            this.IsActive = true;

            // NOTE: Derived classes are responsible for calling
            // gameObject.SetActive(true) inside OnBorrowed().
            this.OnBorrowed();
        }

        /// <summary>
        /// Returns this instance back to the pool.
        /// </summary>
        /// <param name="immediate">If true, object is immediately disabled without calling <see cref="OnReturned"/>.</param>
        public virtual void Return(bool immediate)
        {
            if (immediate)
            {
                this._isReturning = true;
                this.gameObject.SetActive(false);
                return;
            }

            this._isReturning = true;
            this.OnReturned();
            this.Context = default;
        }

        /// <summary>
        /// Unity callback when object becomes disabled.
        /// Used to finalize return to provider.
        /// </summary>
        void OnDisable()
        {
            if (this._provider == null) {
                Debug.LogWarning($"{this.GetType().Name}.OnDisable() provider is null.");
                return;
            }
            
            this.IsActive = false;
            this._provider.Return(this);
        }

        /// <summary>
        /// Called after Borrow.
        /// Derived classes should activate the GameObject here
        /// (gameObject.SetActive(true)).
        /// </summary>
        public abstract void OnBorrowed();

        /// <summary>
        /// Called when Return(false) is requested.
        /// Derived classes may play animations or tweens here.
        /// NOTE: gameObject.SetActive(false) must be called so that
        /// OnDisable returns the object to the provider.
        /// </summary>
        public abstract void OnReturned();

        /// <summary>
        /// Per-frame update while active.
        /// </summary>
        /// <param name="deltaTime">Frame delta time.</param>
        public abstract void OnUpdate(float deltaTime);
    }
}