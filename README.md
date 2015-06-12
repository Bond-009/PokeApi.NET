PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/

How to build:
------------

1. Open the solution in Visual Studio
2. Build it (Press F6/F7/CTRL+SHIFT+B/Hit the 'Build PokeApi.NET'/'Build Solution' button)
3. ?????
4. Profit


To get data from [something]:
-----------------------------

``` cs
using System;
using PokeAPI;
```
[...]
``` cs
Pokemon p = await Pokemon.GetInstance(395); -OR- p = await Pokemon.GetInstance("Lucario");

int baseHp = p.HP;
// etc
```

It works for all types (Pokedex, Pokemon, PokemonType, Move, Ability, Description, Sprite and Game)
