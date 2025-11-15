using DiffEngine;
using Microsoft.Extensions.Logging;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Silksong.Modding.Templates.Tests.ScenarioModel;
using Xunit.Abstractions;

namespace Silksong.Modding.Templates.Tests;

public class SilksongPathTest(ITestOutputHelper output)
{
    private readonly ILogger logger = output.BuildLoggerFor<SilksongPathTest>();

    [Theory]
    [MemberData(nameof(Data))]
    public async Task SnapshotTest(TemplateScenario scenario)
    {
        TemplateVerifierOptions options = new(templateName: "silksongpath")
        {
            TemplatePath = Path.Combine(PathHelper.TemplateContentDir, "SilksongPath"),
            TemplateSpecificArgs = scenario.Args,
            ScenarioName = scenario.ScenarioName,
            DoNotAppendTemplateArgsToScenarioName = true,
        };

        VerificationEngine engine = new(logger);

        await engine.Execute(options);
    }

    public static TheoryData<TemplateScenario> Data =>
        [
            new TemplateScenario("Default", []),
            new TemplateScenario("CustomPath", ["--silksong-install-path", "./My/Local/Path"]),
        ];
}
