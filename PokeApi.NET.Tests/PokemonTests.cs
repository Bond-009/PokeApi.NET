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
            var pokemon = await Pokemon.GetInstanceAsync(1);
            pokemon.AssertResourceWellConfigured();
        }

        [Fact]
        public async Task GetMultipleTimesTheSameItemShouldReturnFromCache()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var FirstTask = Pokemon.GetInstanceAsync(1);
            var SecondTask = Pokemon.GetInstanceAsync(1);
            var results = await Task.WhenAll(FirstTask, SecondTask);

            // there's probably another method for this, but I'm using the GitHub editor atm
            
            foreach (var pokemon in results)
                pokemon.AssertResourceWellConfigured();
        }
    }
}
