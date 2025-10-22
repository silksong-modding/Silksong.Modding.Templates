using BepInEx;

namespace SilksongPlugin._1;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.username.silksongplugin__1")]
public partial class SilksongPlugin__1Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
