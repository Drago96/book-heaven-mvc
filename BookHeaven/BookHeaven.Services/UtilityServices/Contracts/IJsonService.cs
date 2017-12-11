namespace BookHeaven.Services.Utilities
{
    public interface IJsonService : IService
    {
        T DeserializeObject<T>(string value);
    }
}