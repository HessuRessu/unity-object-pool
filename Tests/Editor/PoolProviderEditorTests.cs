using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using UnityEngine.TestTools;
using Pihkura.Pooling.Library;
using Pihkura.Pooling.Data;

namespace Pihkura.Pooling.EditorTests
{
    /// <summary>
    /// EditMode unit tests for the core object pooling system.
    /// 
    /// These tests validate behaviour that does NOT depend on Unity's
    /// runtime MonoBehaviour lifecycle, including:
    /// - Provider initialization
    /// - Factory creation
    /// - Type-keyed reuse
    /// - Error handling
    /// 
    /// GameObject activation, OnDisable callbacks, and lifetime processing
    /// are validated separately in PlayMode tests.
    /// 
    /// Library poolables are not exhaustively tested;
    /// a single representative implementation is used instead.
    /// </summary>
    public class PoolProviderEditorTests
    {
        private static void InvokeAwake(MonoBehaviour behaviour)
        {
            behaviour.GetType()
                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.Invoke(behaviour, null);
        }

        /// <summary>
        /// Creates a PoolProvider with a single PoolableGameObject preset.
        /// </summary>
        private static PoolProvider CreateProvider()
        {
            var providerGO = new GameObject("PoolProvider");
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

            InvokeAwake(provider);

            return provider;
        }

        /// <summary>
        /// Verifies that Get returns a valid instance.
        /// </summary>
        [Test]
        public void Get_Returns_Instance()
        {
            var provider = CreateProvider();

            var ctx = PoolableContext.WithInfinity(Vector3.zero, Quaternion.identity);
            var obj = provider.Get<PoolableGameObject>(ctx);

            Assert.IsNotNull(obj);

            Object.DestroyImmediate(provider.gameObject);
        }

        /// <summary>
        /// Verifies that returned instance is reused.
        /// </summary>
        [Test]
        public void Returned_Object_Is_Reused()
        {
            var provider = CreateProvider();

            var ctx = PoolableContext.WithInfinity(Vector3.zero, Quaternion.identity);

            var a = provider.Get<PoolableGameObject>(ctx);
            provider.Return(a);

            var b = provider.Get<PoolableGameObject>(ctx);

            Assert.AreSame(a, b);

            Object.DestroyImmediate(provider.gameObject);
        }

        /// <summary>
        /// Verifies that requesting unknown type throws.
        /// </summary>
        [Test]
        public void Unknown_Type_Throws()
        {
            LogAssert.ignoreFailingMessages = true;

            var provider = CreateProvider();

            Assert.Throws<System.InvalidOperationException>(() =>
            {
                provider.Get<PoolableLineRenderer>(
                    PoolableContext.WithInfinity(Vector3.zero, Quaternion.identity));
            });

            LogAssert.ignoreFailingMessages = false;

            Object.DestroyImmediate(provider.gameObject);
        }
    }
}
