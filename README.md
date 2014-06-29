PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/

How to build:
------------

1. Open the solution in Visual Studio (10.0 or higher, probably works best in 12.0)
2. Build it (Press F6/F7/CTRL+SHIFT+B/Hit the 'Build PokeApi.NET'/'Build Solution' button)
3. ?????
4. Profit

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
