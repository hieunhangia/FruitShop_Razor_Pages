using System.Text.Json;

namespace FruitShop_Razor_Pages.Extensions;

public static class SessionExtensions
{
    extension(ISession session)
    {
        public void SetObject<T>(string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public T? GetObject<T>(string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}