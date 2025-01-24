using System.Threading.Tasks;
using UnityEngine;

public abstract class DataLoader : IDataLoader
{
    private AssetProvider _assetProvider;

    protected DataLoader(AssetProvider assetProvider)
    {
        _assetProvider = assetProvider;
    }

    public async Task<string> LoadJsonData(string fileName)
    {
        Task<string> task = LoadJsonDataAsync(fileName);
        await task;

        _assetProvider.AddJsonData(fileName, task.Result);

        return task.Result;
    }

    public async Task<T> LoadAsset<T>(string key) where T : Object
    {
        Task<T> task = LoadAssetAsync<T>(key);
        await task;

        _assetProvider.AddAsset(key, task.Result);

        return task.Result;
    }

    protected abstract Task<string> LoadJsonDataAsync(string path);
    protected abstract Task<T> LoadAssetAsync<T>(string key) where T : Object;
}