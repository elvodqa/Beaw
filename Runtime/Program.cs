
using Runtime;
using Engine;
using NLua;

public class Program
{
    public static void Main(string[] args)
    {
        InstanceSettings instanceSettings = new InstanceSettings()
        {
            Size = new(1000, 700),
            Title = "Heaw",
        };

        using (Instance instance = new(instanceSettings))
        {
            instance.LuaApi.ScriptDirectory = args[0];
            instance.LuaApi.LoadApiFunctions();
            instance.LuaApi.LoadInitScript();
            if (instance.LuaApi.Lua["window"] != null)
            {
                LuaTable window = (LuaTable)instance.LuaApi.Lua["window"];
                if (window["width"] != null)
                {
                    Console.WriteLine("window.width: " + window["width"]);
                    instance.InstanceSettings.Size.X = (Int64)window["width"];
                }
                if (window["height"] != null)
                {
                    Console.WriteLine("window.height: " + window["height"]);
                    instance.InstanceSettings.Size.Y = (Int64)window["height"];
                }
                if (window["title"] != null)
                {
                    Console.WriteLine("window.title: " + window["title"]);
                    instance.InstanceSettings.Title = (string)window["title"];
                }
                if (window["fullscreen"] != null)
                {
                    Console.WriteLine("window.fullscreen: " + window["fullscreen"]);
                    instance.InstanceSettings.Fullscreen = (bool)window["fullscreen"];
                }
                // if game.title is not set, use window.title
                if (instance.LuaApi.Lua["game"] != null)
                {
                    LuaTable game = (LuaTable)instance.LuaApi.Lua["game"];
                    if (game["title"] != null)
                    {
                        Console.WriteLine("game.title: " + game["title"]);
                        instance.GameTitle = (string)game["title"];
                    }
                } else {instance.GameTitle = instance.InstanceSettings.Title;}
            }
            instance.Run();
        }
    }
}