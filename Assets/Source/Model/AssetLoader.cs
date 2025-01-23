using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class AssetLoader
{
    private Dictionary<string, Object> _loadedAssets = new Dictionary<string, Object>();

    public async Task<T> LoadAssetAsync<T>(string key) where T : Object
    {
        if (_loadedAssets.TryGetValue(key, out var cachedAsset) && cachedAsset != null)
            return cachedAsset as T;

        try
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedAssets[key] = handle.Result;
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load asset with key {key}, error : {handle.OperationException}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while loading asset {key} : {e}");
            return null;
        }
    }

    public void ReleaseAsset(string key)
    {
        if (_loadedAssets.TryGetValue(key, out var asset))
        {
            Addressables.Release(asset);
            _loadedAssets.Remove(key);
        }
    }
}