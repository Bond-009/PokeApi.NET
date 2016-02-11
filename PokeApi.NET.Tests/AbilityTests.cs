using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class AbilityTests
    {
        [Fact]
        public async Task GetAbilityInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.SetHttpClient(FakeHttpClientAdapter.Singleton);

            var ability = await Ability.GetInstanceAsync(1);

            ability.AssertResourceWellConfigured();
        }
    }
}
