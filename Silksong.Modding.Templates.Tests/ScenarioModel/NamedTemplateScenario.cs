namespace Silksong.Modding.Templates.Tests.ScenarioModel;

public class NamedTemplateScenario(string scenarioName, string inputName, IEnumerable<string> args)
    : TemplateScenario(scenarioName, ["--name", inputName, .. args])
{
    [Obsolete("For serialization use only")]
    public NamedTemplateScenario()
        : this("", "", []) { }
}
