using System;
using Engine;

public class Program
{
    public static void Main(string[] args)
    {
        InstanceSettings instanceSettings = new InstanceSettings()
        {
            Size = new(1000, 700),
            Title = "Heaw"
        };

        using (Instance instance = new(instanceSettings))
        {
            instance.Run();
        }
    }
}