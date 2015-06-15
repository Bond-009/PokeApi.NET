using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PokeAPI.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void GetPokedex()
        {
            var pokedex = AsyncHelpers.RunSync(() => Pokedex.GetInstance());

            Assert.NotNull(pokedex);
            Assert.False(pokedex.AnyStringPropertyEmpty());
        }
        [Fact]
        public void GetPokemonById()
        {
            var pokemon = AsyncHelpers.RunSync(() => Pokemon.GetInstance(1));

            Assert.NotNull(pokemon);
            Assert.False(pokemon.AnyStringPropertyEmpty());
        }
    }
}
