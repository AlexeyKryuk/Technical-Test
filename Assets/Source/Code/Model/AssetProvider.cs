using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetProvider
{
    public const string SaveFileName = "Save.json";
    public const string SettingsFileName = "Settings.json";
    public const string MessageFileName = "Message.json";
    public const string ButtonSpriteName = "ButtonSprite";

    public readonly string Url = Application.streamingAssetsPath;
    public readonly string LocalDataPath = Application.persistentDataPath;

    private Dictionary<string, Object> _loadedAssets = new Dictionary<string, Object>();
    private Dictionary<string, string> _loadedJson = new Dictionary<string, string>();

    public void AddAsset<T>(string name, T asset) where T : Object
    {
        if (_loadedAssets.ContainsKey(name))
            return;

        _loadedAssets.Add(name, asset);
    }

    public void AddJsonData(string name, string json)
    {
        if (_loadedJson.ContainsKey(name))
            return;

        _loadedJson.Add(name, json);
    }

    public void ReleaseAsset(string key)
    {
        if (_loadedAssets.TryGetValue(key, out var asset))
        {
            Addressables.Release(asset);
            _loadedAssets.Remove(key);
        }
    }

    public T GetAsset<T>(string key) where T : Object
    {
        if (_loadedAssets.TryGetValue(key, out var cachedAsset))
            return cachedAsset as T;

        throw new System.IndexOutOfRangeException(key);
    }

    public T GetData<T>(string key)
    {
        if (_loadedJson.TryGetValue(key, out string json))
            return JsonConvert.DeserializeObject<T>(json);

        throw new System.IndexOutOfRangeException(key);
    }
}