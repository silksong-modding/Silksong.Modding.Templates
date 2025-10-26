# Contributing

Here you can find some (hopefully) useful guidance for contributing to the template repo.

## Snapshot testing

The template uses snapshot testing to ensure that the correct output is maintained. Through a few 
[layers of abstraction](https://github.com/dotnet/templating/wiki/Templates-Testing-Tooling),
we ultimately use [Verify](https://github.com/VerifyTests/Verify#readme) for snapshot diffing and management. Running
tests will automatically detect and launch an installed diff tool when any difference is found from the last snapshot.
More information on diff tool detection and ordering can be found on the 
[DiffEngine GitHub page](https://github.com/VerifyTests/DiffEngine#readme).

Note that there is a cap on the number of editors opened to preserve system resources; as such it may be necessary to
run tests multiple times to accept all changes or manage snapshots in another way. Windows users may find it helpful 
to use the DiffEngineTray tool with non-MDI (multi-document interface) diff viewers to more easily manage snapshots 
across many changed files in a run but I found it not that helpful, personally.

### Adding scenarios

All of our tests are parameterized using XUnit Theories. Additional scenarios can be added to the `Data` properties
of the test classes fairly easily. Especially for tests of the project template, this can create very large diffs.
In such cases, rather than using DiffEngine's tooling, it is often easier to manually review the full content of each
file in the scenario's `.received` directory, then rename the directory with the `.verified` suffix.

## Manual testing

Snapshot testing is a good automated verification tool, but it is often desirable to manually test the template as well.
Particularly, manual testing should be used with larger changes to ensure that a project created from the template can
actually be built. This can also help uncover additional inputs which may be suitable for snapshot testing scenarios.

The easiest way to perform manual tests is by installing from a locally build NuGet package, for example:

```sh
dotnet build
dotnet new uninstall Silksong.Modding.Templates
dotnet new install Silksong.Modding.Templates/bin/Debug/Silksong.Modding.Templates.2.0.0.nupkg
```

The template can then be used as normal.