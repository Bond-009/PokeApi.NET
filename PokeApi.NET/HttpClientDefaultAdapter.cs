using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokeAPI
{
    public class HttpClientDefaultAdapter : IHttpClientAdapter
    {
        readonly HttpClient client = new HttpClient();

        public Task<Stream> GetStreamAsync(Uri    requestUri) => client.GetStreamAsync(requestUri);
        public Task<string> GetStringAsync(string requestUri) => client.GetStringAsync(requestUri);

        public Stream GetStreamSync(Uri    requestUri) => client.GetStreamAsync(requestUri).Result;
        public string GetStringSync(string requestUri) => client.GetStringAsync(requestUri).Result;
    }
}
