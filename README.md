PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/

How to build:
------------

1. Open the solution in Visual Studio (12.0 or 14.0, probably works best in 14.0)
2. Build it (Press F6/F7/CTRL+SHIFT+B/Hit the 'Build PokeApi.NET'/'Build Solution' button)
3. ?????
4. Profit

***WARNING: This is made in C# 6.***
***You need either the Roslyn plugin for VS12 or VS14***


To get data from [something]:
-----------------------------

``` cs
using System;
using PokeApi.NET;
```
[...]
``` cs
Pokemon p = Pokemon.GetInstance(395); -OR- p = Pokemon.GetInstance("Lucario");

int baseHp = p.HP;
```

It works for all types (Pokedex, Pokemon, PokemonType, Move, Ability, Description, Sprite and Game)
