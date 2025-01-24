using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    [SerializeField] private LoadingScreenView _loadingScreenView;
    [SerializeField] private CounterScreenView _counterScreenView;

    private AssetProvider _assetProvider;
    private GeneralDataLoader _dataLoader;

    private LoadingDataViewModel _loadingDataViewModel;
    private UpdateDataViewModel _updateDataViewModel;

    private void Awake()
    {
        Compose();
    }

    public async void Compose()
    {
        _assetProvider = new AssetProvider();

        var localLoader = new LocalDataLoader(_assetProvider, _assetProvider.LocalDataPath);
        var remoteLoader = new RemoteDataLoader(_assetProvider, _assetProvider.Url);

        _dataLoader = new GeneralDataLoader(localLoader, remoteLoader);
        _loadingDataViewModel = new LoadingDataViewModel(_loadingScreenView, _dataLoader);

        var loadTask = _loadingDataViewModel.LoadData();

        await loadTask;

        _updateDataViewModel = new UpdateDataViewModel(_counterScreenView, _assetProvider, _dataLoader, localLoader);
        _updateDataViewModel.Initialize();
    }

    private void OnDisable()
    {
        _updateDataViewModel.Dispose();
    }
}