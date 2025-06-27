using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Sources.Runtime.Services.AssetLoader
{
    public class AssetLoader : IAssetLoader
    {
        private readonly DiContainer _container;

        public AssetLoader(DiContainer container) => _container = container;

        public async UniTask<T> Load<T>(string assetId, Transform parent = null) where T : UnityEngine.Object
        {
            var handle = Addressables.InstantiateAsync(assetId, parent);

            var instance = await handle.Task;

            _container.InjectGameObject(instance);

            return await handle.Task as T;
        }

        public async UniTask<Disposable<T>> LoadDisposable<T>(string assetId, Transform parent = null) where T : UnityEngine.Object
        {
            var handle = Addressables.InstantiateAsync(assetId, parent);

            var instance = await handle.Task;

            _container.InjectGameObject(instance);

            return new Disposable<T>(instance as T, () =>
            {
                Addressables.ReleaseInstance(instance);
            });
        }
    }
}