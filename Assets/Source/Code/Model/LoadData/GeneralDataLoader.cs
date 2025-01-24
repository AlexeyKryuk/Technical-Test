using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GeneralDataLoader
{
    private readonly IDataLoader _localLoader;
    private readonly IDataLoader _remoteLoader;

    public GeneralDataLoader(IDataLoader localLoader, IDataLoader remoteLoader)
    {
        _localLoader = localLoader;
        _remoteLoader = remoteLoader;
    }

    public async Task LoadData()
    {
        Task<string> saveTask = _localLoader.LoadJsonData(AssetProvider.SaveFileName);

        var tasks = RemoteData();
        tasks.Add(saveTask);

        await Task.WhenAll(tasks);
    }

    public async Task UpdateData()
    {
        await Task.WhenAll(RemoteData());
    }

    private List<Task> RemoteData()
    {
        List<Task> tasks = new List<Task>
        {
            _remoteLoader.LoadJsonData(AssetProvider.SettingsFileName),
            _remoteLoader.LoadJsonData(AssetProvider.MessageFileName),
            _localLoader.LoadAsset<Sprite>(AssetProvider.ButtonSpriteName)
        };

        return tasks;
    }
}