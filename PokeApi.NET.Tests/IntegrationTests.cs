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

            pokedex.AssertResourceWellConfigured();
        }
        [Fact]
        public void GetPokemonById()
        {
            var pokemon = AsyncHelpers.RunSync(() => Pokemon.GetInstance(1));

            pokemon.AssertResourceWellConfigured();
        }
    }
}
