using PokeAPI;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class EggGroupTests
    {
        [Fact]
        public async Task GetEggGroupInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var eggGroup = await EggGroup.GetInstanceAsync(1);
            eggGroup.AssertResourceWellConfigured();
        }

    }
}
