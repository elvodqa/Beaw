namespace Runtime;

public class Character
{
    public string Name { get; set; }

    public Character(string name)
    {
        Name = name;
    }
    
    public void say(string text)
    {
        Console.WriteLine($"{Name}: {text}");
    }
}