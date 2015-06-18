using PokeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace PokeAPI.Tests
{
    public class FakeHttpClientAdapter : IHttpClientAdapter
    {
        public Task<Stream> GetStreamAsync(Uri requestUri)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            string json = GetJsonFromFile(requestUri);

            return Task.FromResult(json);
        }

        private string GetJsonFromFile(string requestUri)
        {
            var fileName = requestUri.GenerateSlug() + ".json";

            var baseDir = Environment.CurrentDirectory;
            var testFile = Path.Combine(baseDir, "JsonResponses", fileName);
            var json = File.ReadAllText(testFile);
            return json;
        }
    }
}
