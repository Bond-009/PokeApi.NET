PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/. See the `master` branch for the v1 version.

Usage:
-----------------------------

``` cs
using System;
using PokeAPI;

// [...]

Pokemon p = await DataFetcher.GetApiObject<PokemonSpecies>(395); -OR- p = await DataFetcher.GetNamedApiObject<PokemonSpecies>("lucario");

float cRate = p.CaptureRate;
// etc
```

Everything is made to look like the [pokeapi.co docs](http://pokeapi.co/docsv2/), but using more C#-like names. I might add separate methods for every api object type later.
