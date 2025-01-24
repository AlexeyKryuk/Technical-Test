using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalDataLoader : DataLoader, ILocalDataSaver
{
    private readonly string _persistentPath;

    public LocalDataLoader(AssetProvider assetProvider, string persistentPath) : base(assetProvider)
    {
        _persistentPath = persistentPath;
    }

    public async Task SaveJsonDataAsync<T>(T data, string fileName)
    {
        string path = Path.Combine(_persistentPath, fileName);

        try
        {
            string jsonString = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(jsonString);
            }

            Debug.Log("Save data successful");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while saving data to {path} : {e}");
        }
    }

    protected override async Task<string> LoadJsonDataAsync(string fileName)
    {
        string path = Path.Combine(_persistentPath, fileName);

        Debug.Log(path);

        if (!File.Exists(path))
            return string.Empty;

        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string jsonData = await reader.ReadToEndAsync();
                return jsonData;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while loading data from {path} : {e}");
            return string.Empty;
        }
    }

    protected override async Task<T> LoadAssetAsync<T>(string key)
    {
        try
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load asset with key {key}, error : {handle.OperationException}");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while loading asset {key} : {e}");
            return null;
        }
    }
}
