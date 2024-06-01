using System;
using Newtonsoft.Json;

namespace Mod.AccountManager
{
    internal class ServerConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;
        public override bool CanConvert(Type objectType) => typeof(string) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string value)
            {
                return new Server(value);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Server server)
            {
                writer.WriteValue(server.ToString());
            }
        }
    }
}