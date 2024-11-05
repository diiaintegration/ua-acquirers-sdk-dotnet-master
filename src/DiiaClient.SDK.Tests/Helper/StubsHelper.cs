using System;
using System.IO;
using System.Text.Json;

namespace DiiaClient.SDK.Tests.Helper
{
    internal class StubsHelper
    {


        public static T loadAsObject<T>(byte[] file)
        {
            var jsonUtfReader = new Utf8JsonReader(file);
            return JsonSerializer.Deserialize<T>(ref jsonUtfReader);
        }

        public static string LoadAsString(byte[] file)
        {
            if (file == null) throw new FileNotFoundException();
            return System.Text.Encoding.UTF8.GetString(file);
        }

        private StubsHelper()
        {
            throw new InvalidOperationException("Could not create static class StubsHelper.");
        }
    }
}
