using System.Threading.Tasks;
using UnityEngine;

public interface IDataLoader
{
    Task<string> LoadJsonData(string fileName);
    Task<T> LoadAsset<T>(string key) where T : Object;
}