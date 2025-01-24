using System.Threading.Tasks;

public class LoadingDataViewModel
{
    private readonly LoadingScreenView _loadingView;
    private readonly GeneralDataLoader _dataLoader;

    public LoadingDataViewModel(LoadingScreenView loadingView, GeneralDataLoader dataLoader)
    {
        _loadingView = loadingView;
        _dataLoader = dataLoader;
    }

    public async Task LoadData()
    {
        Task task = _dataLoader.LoadData();

        _loadingView.Initialize(task);
        _loadingView.Show();

        await _loadingView.Play();

        _loadingView.Hide();
    }
}
