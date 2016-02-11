using PokeAPI;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class PokedexTests
    {
        [Fact]
        public async Task GetPokedexInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.SetHttpClient(FakeHttpClientAdapter.Singleton);

            var pokedex = await Pokedex.GetInstanceAsync();
            pokedex.AssertResourceWellConfigured();
        }
    }
}
