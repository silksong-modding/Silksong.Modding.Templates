using BepInEx;

namespace SilksongTemplateTester;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.myusername.silksongtemplatetester")]
public partial class SilksongTemplateTesterPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
