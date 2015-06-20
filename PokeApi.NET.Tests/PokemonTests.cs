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

        [Fact]
        public async Task GetMultipleTimesTheSameItemShouldReturnFromCache()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var FirstTask = Pokemon.GetInstance(1);
            var SecondTask = Pokemon.GetInstance(1);
            var results = await Task.WhenAll(FirstTask, SecondTask);

            // this should validate if the item is from the cache, because all are the same.
            var firstHashCode = results[0].GetHashCode();
            foreach (var pokemon in results)
            {
                Assert.Equal(firstHashCode, pokemon.GetHashCode());
                pokemon.AssertResourceWellConfigured();
            }
        }
    }
}
