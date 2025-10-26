using Xunit.Abstractions;

namespace Silksong.Modding.Templates.Tests.ScenarioModel;

public class TemplateScenario(string name, IEnumerable<string> args) : IXunitSerializable
{
    public string ScenarioName { get; private set; } = name;
    public IEnumerable<string> Args { get; private set; } = args;

    [Obsolete("For serialization use only")]
    public TemplateScenario()
        : this("", []) { }

    public void Deserialize(IXunitSerializationInfo info)
    {
        ScenarioName = info.GetValue<string>(nameof(ScenarioName));
        Args = info.GetValue<string[]>(nameof(Args));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(ScenarioName), ScenarioName);
        info.AddValue(nameof(Args), Args.ToArray());
    }
}
