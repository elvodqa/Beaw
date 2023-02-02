namespace Runtime;

public class Character
{
    public Character(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public void say(string text)
    {
        Console.WriteLine($"{Name}: {text}");
    }
}