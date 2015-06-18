using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokeAPI
{
    public class HttpClientDefaultAdapter : IHttpClientAdapter
    {
        HttpClient client = new HttpClient();

        public Task<Stream> GetStreamAsync(Uri requestUri)
        {
            return client.GetStreamAsync(requestUri);
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            return client.GetStringAsync(requestUri);
        }
    }
}
