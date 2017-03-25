using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// 'invalid number' (of next warning suppression)
#pragma warning disable 1692
#pragma warning disable RECS0083
// no 'async' -> block will run synchronously
// not needed, as GetStreamAsync only throws an exn,
// and GetStringAsync creates a Task which result is
// already 'calculated'.
#pragma warning disable 1998

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

        public Stream GetStreamSync(Uri    requestUri)
        {
            throw new NotImplementedException();
        }
        public string GetStringSync(string requestUri) => GetJsonFromFile(requestUri);

        string GetJsonFromFile(string requestUri) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "JsonResponses", requestUri.GenerateSlug() + ".json"));
    }
}
