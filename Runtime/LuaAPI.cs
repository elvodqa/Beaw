using KeraLua;
using Lua = NLua.Lua;

namespace Runtime;

public class LuaAPI
{
    // get first program argument as a directory location
    public string ScriptDirectory = "./";
    public Lua Lua { get; set; } = new();

    public void LoadApiFunctions()
    {
        Lua.LoadCLRPackage();
        Lua.DoString(
            @"
                    import ('Runtime')
                    window = {}
                    game = {}
                    ");
    }

    public void LoadScript(string fileName)
    {
        Lua.DoFile(ScriptDirectory + fileName);
    }

    public void LoadInitScript()
    {
        LoadApiFunctions();
        LoadScript("init.lua");
    }
    
    public void LoadMainScript()
    {
        LoadScript("script.lua");
    }
}