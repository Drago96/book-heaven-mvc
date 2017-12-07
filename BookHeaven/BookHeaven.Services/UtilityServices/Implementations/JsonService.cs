using Newtonsoft.Json;

namespace BookHeaven.Services.Utilities
{
    public class JsonService : IJsonService
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
