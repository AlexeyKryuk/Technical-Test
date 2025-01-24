using System.Threading.Tasks;

public interface ILocalDataSaver
{
    Task SaveJsonDataAsync<T>(T data, string fileName);
}