using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable RECS0083

namespace PokeAPI.Tests
{
    public class FakeHttpClientAdapter : IHttpClientAdapter
    {
        internal readonly static IHttpClientAdapter Singleton = new FakeHttpClientAdapter();

        public Task<Stream> GetStreamAsync(Uri    requestUri)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStringAsync(string requestUri) => Task.FromResult(GetJsonFromFile(requestUri));

        string GetJsonFromFile(string requestUri) => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "JsonResponses", requestUri.GenerateSlug() + ".json"));
    }
}
