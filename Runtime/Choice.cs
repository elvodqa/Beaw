using NLua;

namespace Runtime;

public class Choice
{
    public string Prompt { get; set; }
    public LuaTable Actions { get; set; }
    
    public Choice(string prompt, LuaTable actions)
    {
        Prompt = prompt;
        Actions = actions;
    }
}