# Deprecated
SharpFunction was deprecated! Back when I was actively working on it, I used some very questionable coding practices, such as building JSON for components myself, or creating 10 thousands Command classes. 

Because of that SharpFunction is being migrated to project flux, written in Rust. You can check it out there: https://github.com/Maxuss/flux.

Thanks for understanding!

# SharpFunction

A library for creating datapacks using .NET

SharpFunction's main priority is to simplify the boring work
when making datapacks and focus on interesting parts!

## Installation
SharpFunction is available on [NuGet](https://www.nuget.org/packages/SharpFunction/#).
It is well documented, but if you have any questions, [create an issue](https://github.com/Maxuss/SharpFunction/issues),
or contact me on discord: maxus#8805

## Usage
It is recommended to have functional IDE such as Visual Studio or JetBrains Rider.

You can import SharpFunction after installing, like that:
```csharp
using SharpFunction.API;
using SharpFunction.Universal;
// ...
```

You can also [see examples](https://github.com/Maxuss/SharpFunction/tree/main/SFExample), or [simple guide](https://github.com/Maxuss/SharpFunction/wiki), or just [check full documentation](https://maxuss.github.io/SharpFunction)!

## Addons
 Addons use main library of SharpFunction, to make something more specialized and do not need special installation.
### Skyblock
It is the only current addon for SharpFunction.

It allows creating
* realistic skyblock-like items
* realistic skyblock-like gui parts
* skyblock-like entity summons
And even *whole* slayer, with custom drops, gui items etc.

## Plans for future

Check [github projects](https://github.com/Maxuss/SharpFunction/projects) to see whats coming next!

Current plans:
* ~~Skyblock Addon~~
* ~~Already made project loading~~
* Loot table editing
* Dimension/Worldgen creation
* ???

## License
SharpFunction is licensed under [MIT Licese](https://github.com/Maxuss/SharpFunction/blob/main/LICENSE.txt), and if you will use it, let me know so i can also see what can be made with it!
