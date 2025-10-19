Templates for modding Hollow Knight: Silksong. 

Usage: navigate to the folder you want to create a mod, then run the following:

```
> dotnet new install Silksong.Modding.Templates

> dotnet new silksongplugin
```

By default, the template automatically targets the latest available version of the game. If you want to make mods
specifically targeting an older version of the game, you can use the `-gv` flag to specify the version.

Use `dotnet new silksongplugin --help` to see additional options.