using System.Numerics;
using SDL2;

namespace Engine.Windows;

public class ErrorWindow : Window
{
    public ErrorWindow(string title, Vector2 size) : base(title, size)
    {
    }

    public override void Update(Clock clock)
    {
        base.Update(clock);
    }

    public override void Render()
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 200, 0, 0, 255);
        SDL.SDL_RenderClear(Renderer);
        
        //draw
        
        SDL.SDL_RenderPresent(Renderer);
    }
}