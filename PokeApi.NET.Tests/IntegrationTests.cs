using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokeAPI.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void GetPokedex()
        {
            var pokedex = AsyncHelpers.RunSync(() => Pokedex.GetInstanceAsync());

            pokedex.AssertResourceWellConfigured();
        }
        [Fact]
        public void GetPokemonById()
        {
            var pokemon = AsyncHelpers.RunSync(() => Pokemon.GetInstanceAsync(1));

            pokemon.AssertResourceWellConfigured();
        }
    }
}
