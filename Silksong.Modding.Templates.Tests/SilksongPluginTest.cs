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
        };

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
                "SpecificSilksongVersion",
                "SilksongTemplateTester",
                ["--username", "MyUsername", "--game-version", "1.0.29242"]
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
