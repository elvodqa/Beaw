using System;
using Engine;

public class Program
{
    public static void Main(string[] args)
    {
        InstanceSettings instanceSettings = new InstanceSettings()
        {
            Size = new(100, 10),
            Title = "Hello"
        };

        using (Instance instance = new(instanceSettings))
        {
            instance.Run();
        }
    }
}