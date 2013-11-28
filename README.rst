PokeApi.NET
===========

A .NET Wrapper for http://www.pokeapi.co/

To build it:
------------

Open the solution in Visual Studio (10.0 or higher, optimized for 12.0)

Build it (Press F6/F7/Hit the 'Build PokeApi.NET'/'Build Solution' button)

To get data from [something]:
-----------------------------

    using System;
    using PokeApi.NET;

[...]

    Pokemon p = Pokemon.GetInstance(395);
    p = Pokemon.GetInstance("Lucario");

    int baseHp = p.HP;

It works for all types (Pokedex, Pokemon, PokeType, PokeMove, PokeAbility, PokeDescription, PokeSprite and PokeGame)
