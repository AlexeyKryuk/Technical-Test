using System;
using UnityEngine;

public class UpdateDataViewModel : IDisposable
{
    private readonly CounterScreenView _screenView;
    private readonly AssetProvider _assetProvider;
    private readonly GeneralDataLoader _dataLoader;
    private readonly ILocalDataSaver _localDataSaver;

    private SaveData _saveData;
    private Settings _settings;
    private Sprite _buttonSprite;
    private Message _message;

    public UpdateDataViewModel(CounterScreenView screenView, AssetProvider assetProvider, GeneralDataLoader dataLoader, ILocalDataSaver localDataSaver)
    {
        _screenView = screenView;
        _assetProvider = assetProvider;
        _dataLoader = dataLoader;
        _localDataSaver = localDataSaver;
    }

    public void Initialize()
    {
        _screenView.OnIncrementClick += IncrementCounter;
        _screenView.OnReloadClick += ReloadData;

        LoadData();

        _saveData = _assetProvider.GetData<SaveData>(AssetProvider.SaveFileName);

        int startingNumber = _saveData?.Counter ?? _settings.StartingNumber;
        
        if (_saveData == null)
            _saveData = new SaveData(startingNumber);

        _screenView.Initialize(_buttonSprite, _message.Text, startingNumber);
        _screenView.Show();
    }

    public void Dispose()
    {
        _localDataSaver.SaveJsonDataAsync(_saveData, AssetProvider.SaveFileName);

        _screenView.OnIncrementClick -= IncrementCounter;
        _screenView.OnReloadClick -= ReloadData;
    }

    private void LoadData()
    {
        _buttonSprite = _assetProvider.GetAsset<Sprite>(AssetProvider.ButtonSpriteName);
        _message = _assetProvider.GetData<Message>(AssetProvider.MessageFileName);
        _settings = _assetProvider.GetData<Settings>(AssetProvider.SettingsFileName);
    }

    private void IncrementCounter()
    {
        _saveData.Counter++;
        _screenView.UpdateCounter(_saveData.Counter);
    }

    private async void ReloadData()
    {
        await _dataLoader.UpdateData();

        LoadData();
        _screenView.Initialize(_buttonSprite, _message.Text, _saveData.Counter);
    }
}