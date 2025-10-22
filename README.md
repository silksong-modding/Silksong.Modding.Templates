# Silksong.Modding.Templates

Templates for modding Hollow Knight: Silksong. 

Usage: navigate to the folder you want to create a mod, then run the following:

```
> dotnet new install Silksong.Modding.Templates

> dotnet new silksongplugin --username YourGitHubUsername
```

By default, the template automatically targets the latest available version of the game. If you want to make mods
specifically targeting an older version of the game, you can use the `-gv` flag to specify the version.

Use `dotnet new silksongplugin --help` to see additional options.

## What's included in the template?

The template creates the following items for you:

- A basic plugin class for your mod
- A Directory.Build.props file where you can set metadata shared between Thunderstore and the build system in a single place
- A SilksongPath.props file making it easy to point the build system at your local files without committing local paths to Git
- A project file that automatically copies your mod to your game folder and creates a Thunderstore
  package when building your mod using [Thunderstore CLI](https://github.com/thunderstore-io/thunderstore-cli)
- Placeholder metadata for packaging your mod in Thunderstore, including a lovely default icon by @Bigfootmech
- A GitHub actions workflow to help automatically publish your mod to GitHub Releases, Thunderstore, and/or NuGet.
  Minor additional setup is required for publishing; see steps below.


## Items that might need to be modified

There are a few scenarios where the template makes a best guess. The following things may need to be changed
in some scenarios:

- `website_url` in thunderstore.toml and `RepositoryUrl` in the .csproj assume that you are using GitHub and that the
  repository name is the safe class name of your project. If either is not the case, you will have to update these URLs.
- Most metadata contained in thunderstore.toml is placeholder. You will have to update it before publishing your mod.
- The Thunderstore icon should minimally be updated to contain your mod name; there is space available for top/bottom text.
  Of course, you can also replace it with any .png of the same size.
- The `id` field in the plugin class similarly assumes that you are using GitHub. This ID is the BepInEx plugin GUID
  which means it should be globally unique and should not be changed after your first release. As a best practice, it
  is recommended to prefix the ID with the reverse of a domain name you own. All GitHub users have `username.github.io`
  for free by default, so the default prefix is `io.github.username`.

## Publishing with GitHub Actions

The template contains a workflow to automatically publish your mod whenever you update the version in Directory.Build.props.
The workflow is defined in `.github/workflows/build-publish.yml`. By default, publishing is disabled so that you can iterate
on your mod freely before the initial release. When you are ready to make your first release, simply change the `allow-release`
output from `'false'` to `'true'` (make sure to configure Thunderstore and NuGet first if desired). 

Publishing to Thunderstore and NuGet requires you to create API keys for those destinations. Without setting these API keys,
publishing to those destinations is skipped (ie, the workflow will not fail). API keys are secret, and should be 
treated similarly to a password. You can follow [GitHub's documentation] for how to add a secret to a repository. Steps for
configuring the necessary API keys can be found in the next sections.

### Publishing to GitHub Releases

Publishing to GitHub releases is enabled by default. It's recommended to enable release immutability from the General
tab of your repository settings.

If you want to change the format of your release notes, you can do so by updating the `release-github` job.
Documentation for the release action can be found at https://github.com/softprops/action-gh-release.

### Publishing to Thunderstore

Thunderstore is the recommended place to publish most mods. In order to publish to Thunderstore, you will need to set
the `THUNDERSTORE_API_KEY` secret on your repository. To get a Thunderstore API key, do the following steps:

1. Go to [your teams](https://thunderstore.io/settings/teams/) on Thunderstore and create one if needed.
   It's common for most developers to have a single team which is the same as their username on Thunderstore.
2. Click the team that you will publish the package under, and click Service Accounts.
3. Add a new service account. It's recommended to do this once per repository so that if your token is accidentally leaked,
   you can revoke it without impacting your other mods' build workflows.
4. Copy the provided API key and add it to your repository as a secret named `THUNDERSTORE_API_KEY`.

Before attempting to publish, it's a good idea to check that your manifest is valid. Thunderstore provides a manifest validator 
[here](https://thunderstore.io/tools/manifest-v1-validator/). Your manifest is automatically generated during build,
and can be found in the zip file in the `thunderstore/dist` directory.

### Publishing to NuGet

If you intend for your mod to be used primarily as a dependency, it's also recommended to publish your mod to NuGet
so that consumers can use it in workflows like this one. In order to publish to NuGet you will need to set the
`NUGET_API_KEY` secret on your repository. To get a NuGet API key, do the following steps:

1. Determine the NuGet package name for your mod. The easiest way to do this is to run `dotnet pack` and check the output
   file name. Usually, it will be the same as your project name. If you want to use a different name when publishing to NuGet,
   you can add a `<PackageId>` tag to your Directory.Build.props.
2. Follow [NuGet's documentation](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package#create-an-api-key)
   for creating an API key. Rather than entering `*` as the Glob Pattern, enter the package name you found in step 1.
   It's recommended to do this once per repository so that if your token is accidentally leaked, you can revoke it without
   impacting your other mods' build workflows.
3. Copy the provided API key and add it to your repository as a secret named `NUGET_API_KEY`.


## Publishing manually

When using this template, automated publishing via GitHub Actions should be preferred in most cases. Some reasons that might
warrant manual publishing include:

- Publishing to GitHub releases succeeded, but Thunderstore and/or NuGet failed, such as if you forgot to set API keys 
  before enabling your first release
- You aren't using GitHub to host your repository

There are 2 ways to manually publish; using a workflow dispatch or via a terminal.

### Manually publishing via workflow dispatch

The workflow comes pre-configured with a workflow dispatch trigger that can forcibly re-publish to Thunderstore and/or
NuGet. To do this, navigate to your repository's Actions tab and open the page for the Build workflow. Then, click the
"Run workflow" dropdown and select the destinations you want to republish to. You will still need to configure API keys
(as described above) for publishing to succeed.

### Manually publishing via a terminal

The build workflow can serve as a reference for the steps you need to do.

- `dotnet build` will build your mod and create a Thunderstore package in `thunderstore/dist`
- `dotnet pack -o nuget` will create a NuGet package in the `nuget` directory

Once these artifacts are created, you can upload them either by using the respective websites' upload functionality,
or you can use the same CLI commands used in the publish jobs.
