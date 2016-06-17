PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/. See the `master` branch for the v1 version.

Usage:
-----------------------------

### C#

``` cs
using System;
using PokeAPI;

// [...]

// in async method

PokemonSpecies p = await DataFetcher.GetApiObject<PokemonSpecies>(395);
// or:
PokemonSpecies p = await DataFetcher.GetNamedApiObject<PokemonSpecies>("lucario");

float cRate = p.CaptureRate;
// etc
```

To get the value behind the `Task<T>` object synchronously, use the `Result` property.

### F#

``` fs
open System
open PokeAPI

// [...]

async
{
    let! p = DataFetcher.GetApiObject<PokemonSpecies> 395 |> Async.AwaitTask;
    // or:
    let! p = DataFetcher.GetNamedApiObject<PokemonSpecies> "lucario" |> Async.AwaitTask;

    let cRate = p.CaptureRate;
    // etc
}
```

To get the value behind the `Async<T>` object synchronously, use the `Async.RunSynchronously` function.
If it's a `Task<T>`, do as described under 'C#'.

## Docs

Separate docs for this library aren't really needed,
everything is made to look like the [pokeapi.co docs](http://pokeapi.co/docsv2/),
but using more C#-like names. I might add separate methods for every api object type later.
