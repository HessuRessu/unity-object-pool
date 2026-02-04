using UnityEngine;

namespace Pihkura.Pooling.Data
{
    /// <summary>
    /// Context passed when borrowing a pooled object.
    /// Contains spatial data and optional lifetime.
    /// </summary>
    public struct PoolableContext
    {
        /// <summary>
        /// Lifetime in seconds.
        /// Negative value means infinite lifetime and user must call Return manually.
        /// </summary>
        public float lifeTime;

        /// <summary>
        /// Desired local scale of the object.
        /// </summary>
        public Vector3 scale;

        /// <summary>
        /// World-space position.
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// World-space rotation.
        /// </summary>
        public Quaternion rotation;

        /// <summary>
        /// Optional parent transform for the borrowed object.
        /// </summary>
        public Transform parent;

        /// <summary>
        /// Optional color for the borrowed object.
        /// </summary>
        public Color color;

        /// <summary>
        /// Creates a context with infinite lifetime.
        /// </summary>
        /// <param name="position">World-space position.</param>
        /// <param name="rotation">World-space rotation.</param>
        /// <returns>Initialized <see cref="PoolableContext"/> with infinite lifetime.</returns>
        public static PoolableContext WithInfinity(Vector3 position, Quaternion rotation)
            => WithInfinity(position, rotation, Color.black);

        /// <summary>
        /// Creates a context with infinite lifetime.
        /// </summary>
        /// <param name="position">World-space position.</param>
        /// <param name="rotation">World-space rotation.</param>
        /// <param name="color">Color for poolable.</param>
        /// <returns>Initialized <see cref="PoolableContext"/> with infinite lifetime.</returns>
        public static PoolableContext WithInfinity(Vector3 position, Quaternion rotation, Color color)
        {
            return new PoolableContext()
            {
                lifeTime = -1f,
                scale = Vector3.one,
                position = position,
                rotation = rotation,
                color = color,
            };
        }

        /// <summary>
        /// Creates a context with a finite lifetime.
        /// </summary>
        /// <param name="lifeTime">Lifetime in seconds.</param>
        /// <param name="position">World-space position.</param>
        /// <param name="rotation">World-space rotation.</param>
        /// <returns>Initialized <see cref="PoolableContext"/> with finite lifetime.</returns>
        public static PoolableContext WithFinity(float lifeTime, Vector3 position, Quaternion rotation)
            => WithFinity(lifeTime, position, rotation, Color.black);

        /// <summary>
        /// Creates a context with a finite lifetime.
        /// </summary>
        /// <param name="lifeTime">Lifetime in seconds.</param>
        /// <param name="position">World-space position.</param>
        /// <param name="rotation">World-space rotation.</param>
        /// <param name="color">Color for poolable.</param>
        /// <returns>Initialized <see cref="PoolableContext"/> with finite lifetime.</returns>
        public static PoolableContext WithFinity(float lifeTime, Vector3 position, Quaternion rotation, Color color)
        {
            return new PoolableContext()
            {
                lifeTime = lifeTime,
                scale = Vector3.one,
                position = position,
                rotation = rotation,
                color = color
            };
        }
    }
}