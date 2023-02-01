using NLua;

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
                    ");
    }

    public void LoadScript(string fileName)
    {
        Lua.DoFile(fileName);
    }

    public void LoadInitScript()
    {
        LoadApiFunctions();
        LoadScript(ScriptDirectory + "init.lua");
        LoadScript(ScriptDirectory + "script.lua");
    }
    
    
}