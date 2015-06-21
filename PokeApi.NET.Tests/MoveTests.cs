using PokeAPI;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class MoveTests
    {
        [Fact]
        public async Task GetMoveInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var move = await Move.GetInstanceAsync(1);
            move.AssertResourceWellConfigured();
        }

    }
}
