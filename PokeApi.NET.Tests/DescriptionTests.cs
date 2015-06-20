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
            DataFetcher.client = new FakeHttpClientAdapter();
            var description = await Description.GetInstanceAsync(1);
            description.AssertResourceWellConfigured();
        }

    }
}
