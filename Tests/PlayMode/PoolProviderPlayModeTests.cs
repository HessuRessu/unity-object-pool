using NUnit.Framework;
using UnityEngine;
using Pihkura.Pooling.Library;
using Pihkura.Pooling.Data;
using System.Collections;
using UnityEngine.TestTools;

namespace Pihkura.Pooling.PlayTests
{
    /// <summary>
    /// PlayMode lifecycle tests for the core object pooling system.
    /// 
    /// These tests specifically validate behaviour that depends on Unity's
    /// runtime MonoBehaviour lifecycle, including:
    /// - GameObject activation / deactivation
    /// - OnDisable invocation
    /// - Lifetime-based auto return driven by Update()
    /// 
    /// Pure data and factory behaviour is covered by EditMode tests.
    /// 
    /// Library poolables are not exhaustively tested;
    /// a single representative implementation is used instead.
    /// </summary>
    public class PoolProviderPlayModeTests
    {
        /// <summary>
        /// Creates a fully configured <see cref="PoolProvider"/> instance
        /// for PlayMode testing.
        /// 
        /// The provider GameObject is temporarily disabled to prevent Awake
        /// from executing before required fields are assigned.
        /// </summary>
        /// <returns>Initialized PoolProvider instance.</returns>
        private static PoolProvider CreateProvider()
        {
            var providerGO = new GameObject("PoolProvider");
            providerGO.SetActive(false); // Prevent Awake from running before fields are assigned

            var parentGO = new GameObject("PoolRoot");

            var provider = providerGO.AddComponent<PoolProvider>();
            provider.poolParent = parentGO.transform;

            var prefabGO = new GameObject("PoolablePrefab");
            var poolable = prefabGO.AddComponent<PoolableGameObject>();

            provider.presets = new PoolablePrefab[]
            {
                new PoolablePrefab()
                {
                    prefab = poolable,
                    prewarmCount = 1
                }
            };

            providerGO.SetActive(true); // Awake runs here with valid configuration

            return provider;
        }

        /// <summary>
        /// Verifies that <see cref="IPoolable.IsActive"/> is set to true on borrow
        /// and false after return when running in PlayMode.
        /// </summary>
        [UnityTest]
        public IEnumerator IsActive_Toggles_Correctly()
        {
            var provider = CreateProvider();
            yield return null;  // Allow one frame for initialization

            var obj = provider.Get<PoolableGameObject>(
                PoolableContext.WithInfinity(Vector3.zero, Quaternion.identity));

            Assert.IsTrue(obj.IsActive);

            obj.Return(true);

            yield return null; // allow OnDisable

            Assert.IsFalse(obj.IsActive);

            Object.Destroy(provider.gameObject);
        }

        /// <summary>
        /// Verifies that an object with a finite lifetime is automatically
        /// returned to the pool after its lifetime elapses.
        /// </summary>
        [UnityTest]
        public IEnumerator Lifetime_Auto_Returns_Object()
        {
            var provider = CreateProvider();
            yield return null; // Allow one frame for initialization

            var obj = provider.Get<PoolableGameObject>(
                PoolableContext.WithFinity(0.25f, Vector3.zero, Quaternion.identity));

            Assert.IsTrue(obj.IsActive);

            yield return new WaitForSeconds(0.3f);

            Assert.IsFalse(obj.IsActive);

            Object.Destroy(provider.gameObject);
        }

    }
}
