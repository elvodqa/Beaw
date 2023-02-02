using System.Numerics;
using Engine;
using NLua;
using Runtime;

public class Program
{
    public static void Main(string[] args)
    {
        var instanceSettings = new InstanceSettings
        {
            Size = new Vector2(1000, 700),
            Title = "Heaw"
        };

        using (Instance instance = new(instanceSettings))
        {
            instance.LuaApi.ScriptDirectory = args[0];
            instance.LuaApi.LoadApiFunctions();
            instance.LuaApi.LoadInitScript();
            if (instance.LuaApi.Lua["window"] != null)
            {
                var window = (LuaTable)instance.LuaApi.Lua["window"];
                if (window["width"] != null)
                {
                    Console.WriteLine("window.width: " + window["width"]);
                    instance.InstanceSettings.Size.X = (long)window["width"];
                }

                if (window["height"] != null)
                {
                    Console.WriteLine("window.height: " + window["height"]);
                    instance.InstanceSettings.Size.Y = (long)window["height"];
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
                    var game = (LuaTable)instance.LuaApi.Lua["game"];
                    if (game["title"] != null)
                    {
                        Console.WriteLine("game.title: " + game["title"]);
                        instance.GameTitle = (string)game["title"];
                    }
                }
                else
                {
                    instance.GameTitle = instance.InstanceSettings.Title;
                }
            }

            instance.Run();
        }
    }
}