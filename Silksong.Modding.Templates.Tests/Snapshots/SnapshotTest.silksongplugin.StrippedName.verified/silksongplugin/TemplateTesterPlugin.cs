using BepInEx;

namespace Silksong.TemplateTester;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.myusername.templatetester")]
public partial class TemplateTesterPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
