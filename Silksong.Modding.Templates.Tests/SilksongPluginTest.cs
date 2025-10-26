using Microsoft.Extensions.Logging;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace Silksong.Modding.Templates.Tests;

public class SilksongPluginTest(ITestOutputHelper output)
{
    private readonly ILogger logger = output.BuildLoggerFor<SilksongPluginTest>();

    [Theory]
    [MemberData(nameof(Data))]
    public async Task SnapshotTest(ProjectTemplateScenario scenario)
    {
        TemplateVerifierOptions options = new(templateName: "silksongplugin")
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

    public static TheoryData<ProjectTemplateScenario> Data =>
        [
            new ProjectTemplateScenario("Minimal", ["--username", "MyUsername"]),
            new ProjectTemplateScenario(
                "ThunderstoreOverride",
                ["--username", "MyUsername", "--thunderstore-username", "my_username"]
            ),
            new ProjectTemplateScenario(
                "StrippedName",
                ["--username", "MyUsername"],
                nameOverride: "Silksong.TemplateTester"
            ),
        ];
}
