using System.Threading.Tasks;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;

namespace Game.Hotfix.Framework
{
    public static class ResourceComponentExtension
    {
        public static Task<T> LoadAssetAsync<T>(this ResourceComponent resourceComponent, string assetName) where T : UnityEngine.Object
        {
            TaskCompletionSource<T> loadAssetTcs = new TaskCompletionSource<T>();
            resourceComponent.LoadAsset(assetName, typeof(T), new LoadAssetCallbacks(
                (tempAssetName, asset, duration, userdata) =>
                {
                    var source = loadAssetTcs;
                    loadAssetTcs = null;
                    T tAsset = asset as T;
                    if (tAsset != null)
                    {
                        source.SetResult(tAsset);
                    }
                    else
                    {
                        Debug.LogError($"Load asset failure load type is {asset.GetType()} but asset type is {typeof(T)}.");
                        source.SetException(new GameFrameworkException(
                            $"Load asset failure load type is {asset.GetType()} but asset type is {typeof(T)}."));
                    }
                },
                (tempAssetName, status, errorMessage, userdata) =>
                {
                    Debug.LogError(errorMessage);
                    loadAssetTcs.SetException(new GameFrameworkException(errorMessage));
                }
            ));

            return loadAssetTcs.Task;
        }

        public static void LoadAsset<T>(this ResourceComponent resourceComponent, string assetName, UnityAction<T> onSuccess) where T : UnityEngine.Object
        {
            resourceComponent.LoadAsset(assetName, typeof(T), new LoadAssetCallbacks(
                (tempAssetName, asset, duration, userdata) =>
                {
                    T tAsset = asset as T;
                    if (tAsset != null)
                    {
                        onSuccess?.Invoke(tAsset);
                    }
                    else
                    {
                        Debug.LogError($"Load asset failure load type is {asset.GetType()} but asset type is {typeof(T)}.");
                    }
                },
                (tempAssetName, status, errorMessage, userdata) =>
                {
                    Debug.LogError(errorMessage);
                }
            ));
        }
    }

}