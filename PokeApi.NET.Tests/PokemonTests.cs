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

            foreach (var pokemon in results)
            {
                pokemon.AssertResourceWellConfigured();
            }

            //Parallel.For(0, 2, async action =>
            //{
            //    var pokemon = await Pokemon.GetInstance(1);
            //    pokemon.AssertResourceWellConfigured();
            //});
        }

    }
}
