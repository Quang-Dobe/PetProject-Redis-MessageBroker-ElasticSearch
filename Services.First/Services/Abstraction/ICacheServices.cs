namespace Services.First.Services.Abstraction
{
    public interface ICacheServices
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        bool RemoveData(string key);
    }
}
