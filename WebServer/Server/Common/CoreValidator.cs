namespace MyCoolWebServer.Server.Common
{
    using System;

    public static class CoreValidator
    {
        // name - name of parameter to check
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"{name} cannot be null or empty", name);
            }
        }
    }
}
