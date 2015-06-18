using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class AbilityTests
    {
        [Fact]
        public async Task GetAbilityInstanceByIdDeserializeAllProperties()
        {
            DataFetcher.client = new FakeHttpClientAdapter();
            var ability = await Ability.GetInstance(1);

            ability.AssertResourceWellConfigured();
        }
    }
}
