using System;
using System.Collections.Generic;
using Pihkura.Pooling.Data;
using Pihkura.Pooling.Implementations;
using UnityEngine;

namespace Pihkura.Pooling
{
    /// <summary>
    /// Central manager responsible for creating, storing, and reusing pooled objects.
    /// </summary>
    public class PoolProvider : MonoBehaviour
    {
        [Header("Pool configuration")]

        /// <summary>
        /// Parent transform under which pooled objects are organized.
        /// </summary>
        public Transform poolParent;

        /// <summary>
        /// List of prefab presets used by this provider.
        /// </summary>
        public PoolablePrefab[] presets;

        private Dictionary<Type, Stack<BasePoolable>> _reserve = new Dictionary<Type, Stack<BasePoolable>>();

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static PoolProvider Instance { get; private set; }

        /// <summary>
        /// Unity Awake. Initializes singleton and prewarms pools.
        /// </summary>
        void Awake()
        {
            if (Instance == null)
            {
                if (this.poolParent == null)
                    throw new InvalidOperationException("poolParent is null, consider setting poolParent.");

                Instance = this;
                this.Prewarm();
                return;
            }
#if UNITY_EDITOR
            DestroyImmediate(this.gameObject);
#else
            Destroy(this.gameObject);
#endif
        }

        /// <summary>
        /// Borrows a pooled object of type T.
        /// </summary>
        /// <typeparam name="T">Concrete poolable type.</typeparam>
        /// <param name="context">Borrow context.</param>
        /// <returns>Borrowed instance of type T.</returns>
        public T Get<T>(PoolableContext context) where T : BasePoolable
        {
            T typedPoolable = null;
            Type type = typeof(T);

            if (!this._reserve.TryGetValue(type, out Stack<BasePoolable> stack))
            {
                stack = new Stack<BasePoolable>();
                this._reserve.Add(type, stack);
            }

            if (stack.TryPop(out BasePoolable candidate))
            {
                typedPoolable = candidate as T;
            }

            if (typedPoolable == null)
            {
                if (this.TryGetFactory<T>(out T poolable))
                {
                    typedPoolable = poolable;
                }
                else
                {
                    throw new InvalidOperationException($"There is no factory preset for {typeof(T)} poolable type!");
                }
            }

            typedPoolable.Borrow(context);
            return typedPoolable;
        }

        /// <summary>
        /// Attempts to create a new instance using preset factories.
        /// </summary>
        /// <typeparam name="T">Concrete poolable type.</typeparam>
        /// <param name="poolable">Created instance if successful.</param>
        /// <returns>True if a matching preset was found.</returns>
        private bool TryGetFactory<T>(out T poolable) where T : BasePoolable
        {
            for (int i = 0; i < this.presets.Length; i++)
            {
                if (this.presets[i].prefab is T typedCandidate)
                {
                    poolable = Instantiate(typedCandidate, this.poolParent, false);
                    poolable.Initialize(this);
                    return true;
                }
            }

            poolable = null;
            return false;
        }

        /// <summary>
        /// Returns an instance back to the reserve stack.
        /// </summary>
        /// <param name="poolable">Instance being returned.</param>
        public void Return(BasePoolable poolable)
        {
            Type type = poolable.GetType();

            if (!this._reserve.TryGetValue(type, out Stack<BasePoolable> stack))
            {
                stack = new Stack<BasePoolable>();
                this._reserve.Add(type, stack);
            }

            if (!stack.Contains(poolable))
                stack.Push(poolable);
        }

        /// <summary>
        /// Returns all instances back to the reserve stack.
        /// </summary>
        /// <param name="immediate">If true, object is immediately disabled without calling <see cref="OnReturned"/>.</param>
        public void ReturnAll(bool immediate)
        {
            BasePoolable[] poolables = GameObject.FindObjectsByType<BasePoolable>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            foreach (BasePoolable poolable in poolables)
                if (poolable.IsActive)
                    poolable.Return(immediate);
        }

        /// <summary>
        /// Pre-creates instances according to preset configuration.
        /// </summary>
        private void Prewarm()
        {
            for (int i = 0; i < this.presets.Length; i++)
            {
                for (int n = 0; n < this.presets[i].prewarmCount; n++)
                {
                    BasePoolable poolable = Instantiate(this.presets[i].prefab, this.poolParent, false);
                    poolable.Initialize(this);
                    poolable.gameObject.SetActive(false);
                    this.Return(poolable);
                }
            }
        }
    }
}
