using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeAPI
{
    public interface IHttpClientAdapter
    {
        Task<string> GetStringAsync(string requestUri);
        Task<Stream> GetStreamAsync(Uri requestUri);

        string GetStringSync(string requestUri);
        Stream GetStreamSync(Uri    requestUri);
    }
}
