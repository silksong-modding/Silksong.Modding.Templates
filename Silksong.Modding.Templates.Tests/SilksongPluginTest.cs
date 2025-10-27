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
            // https://github.com/silksong-modding/Silksong.Modding.Templates/actions/runs/18831278154/job/53723055893
            ScrubbersDefinition.Empty.AddScrubber(content => content.Replace("{TempPath}", "/tmp"))
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
