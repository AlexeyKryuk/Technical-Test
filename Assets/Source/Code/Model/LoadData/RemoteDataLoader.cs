using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteDataLoader : DataLoader
{
    private readonly string _url;

    public RemoteDataLoader(AssetProvider assetProvider, string url) : base(assetProvider)
    {
        _url = url;
    }

    protected override async Task<string> LoadJsonDataAsync(string fileName)
    {
        string path = Path.Combine("file://", _url, fileName);

        Debug.Log(path);

        try
        {
            using (var www = UnityWebRequest.Get(path))
            {
                var task = www.SendWebRequest();

                while (!task.isDone)
                    await Task.Yield();

                if (www.result == UnityWebRequest.Result.Success)
                {

                    string jsonString = www.downloadHandler.text;
                    return jsonString;
                }
                else
                {
                    Debug.LogError("Failed to load JSON: " + www.error);
                    return default;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading {path} : {e}");
            return default;
        }
    }

    protected override Task<T> LoadAssetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }
}