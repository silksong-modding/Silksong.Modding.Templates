using Microsoft.Extensions.Logging;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Silksong.Modding.Templates.Tests.ScenarioModel;
using Xunit.Abstractions;

namespace Silksong.Modding.Templates.Tests;

public class SilksongPluginTest(ITestOutputHelper output)
{
    private readonly ILogger logger = output.BuildLoggerFor<SilksongPluginTest>();

    [Theory]
    [MemberData(nameof(Data))]
    public async Task SnapshotTest(NamedTemplateScenario scenario)
    {
        TemplateVerifierOptions options = new TemplateVerifierOptions(
            templateName: "silksongplugin"
        )
        {
            TemplatePath = Path.Combine(PathHelper.TemplateContentDir, "SilksongPlugin"),
            TemplateSpecificArgs = scenario.Args,
            ScenarioName = scenario.ScenarioName,
            DoNotAppendTemplateArgsToScenarioName = true,
            VerificationExcludePatterns = PathHelper.ContentRelativeGlobs(
                ".gitignore",
                "packages.lock.json",
                "thunderstore/icon.png"
            ),
        }.WithCustomScrubbers(
            // on Linux, the `thunderstore/tmp` path is erroneously replaced with `thunderstore{TempPath}` with disastrous consequences
            // https://github.com/silksong-modding/Silksong.Modding.Templates/actions/runs/18831278154/job/53723055893?pr=46
            ScrubbersDefinition
                .Empty.AddScrubber(sb => sb.Replace("thunderstore{TempPath}", "/tmp"), "csproj")
                .AddScrubber(
                    (path, content) =>
                    {
                        if (Path.GetFileName(path) == "thunderstore.toml")
                        {
                            content.Replace("\".{TempPath}\"", "\"./tmp\"");
                        }
                    }
                )
        );

        VerificationEngine engine = new(logger);

        await engine.Execute(options);
    }

    public static TheoryData<NamedTemplateScenario> Data =>
        [
            new NamedTemplateScenario(
                "Minimal",
                "SilksongTemplateTester",
                ["--username", "MyUsername"]
            ),
            new NamedTemplateScenario(
                "ThunderstoreOverride",
                "SilksongTemplateTester",
                ["--username", "MyUsername", "--thunderstore-username", "my_username"]
            ),
            new NamedTemplateScenario(
                "StrippedName",
                "Silksong.TemplateTester",
                ["--username", "MyUsername"]
            ),
        ];
}
