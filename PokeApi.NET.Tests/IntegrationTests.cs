using PokeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokeApi.NET.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task GetPokedex()
        {
            var pokedex = await Pokedex.GetInstance();

            Assert.NotNull(pokedex);
            Assert.False(pokedex.AnyStringPropertyNullOrEmpty());
        }

        [Fact]
        public async Task GetPokemonById()
        {
            var pokemon = await Pokemon.GetInstance(1);

            Assert.NotNull(pokemon);
            Assert.False(pokemon.AnyStringPropertyNullOrEmpty());
        }
    }
}
