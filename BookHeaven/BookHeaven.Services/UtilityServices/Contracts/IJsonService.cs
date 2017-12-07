namespace BookHeaven.Services.Utilities
{
    public interface IJsonService : IServce
    {
        T DeserializeObject<T>(string value);
    }
}
