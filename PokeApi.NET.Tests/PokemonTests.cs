using PokeAPI;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class PokemonTests
    {
        [Fact]
        public async Task GetPokemonInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var pokemon = await Pokemon.GetInstance(1);
            pokemon.AssertResourceWellConfigured();
        }

    }
}
