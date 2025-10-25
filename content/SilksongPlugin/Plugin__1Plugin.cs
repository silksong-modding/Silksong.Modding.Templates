using BepInEx;

namespace Silksong.Plugin._1;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.username.plugin__1")]
public partial class Plugin__1Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
