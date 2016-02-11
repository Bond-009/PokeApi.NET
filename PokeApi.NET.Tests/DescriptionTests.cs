using PokeAPI;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class DescriptionTests
    {
        [Fact]
        public async Task GetDescriptionInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.SetHttpClient(FakeHttpClientAdapter.Singleton);

            var description = await Description.GetInstanceAsync(1);
            description.AssertResourceWellConfigured();
        }
    }
}
