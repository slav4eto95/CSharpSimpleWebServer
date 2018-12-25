using MyCoolWebServer.Server.Common;

namespace MyCoolWebServer.Server.Http
{
    public class HttpHeader
    {
        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Key = key;
            Value = value;
        }

        public override string ToString() => $"{Key}: {Value}";
        
        public string Key { get; private set; }
        public string Value { get; private set; }

    }
}
