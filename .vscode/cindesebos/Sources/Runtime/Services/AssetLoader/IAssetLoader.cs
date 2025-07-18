using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.Runtime.Services.AssetLoader
{
    public interface IAssetLoader
    {
        UniTask<T> Load<T>(string assetId, Transform parent = null) where T : UnityEngine.Object;

        UniTask<Disposable<T>> LoadDisposable<T>(string assetId, Transform parent = null) where T : UnityEngine.Object;
    }
}