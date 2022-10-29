using System.Numerics;
using SDL2;

namespace Engine;

public class Window
{
    public IntPtr Handle;
    public Vector2 Size;
    public string Title;
    public IntPtr Renderer;
    
    private int _x;
    private int _y;

    public Window(string title, Vector2 size)
    {
        Title = title;
        Size = size;
        Handle = SDL.SDL_CreateWindow(
            Title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            (int)Size.X,
            (int)Size.Y,
            SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI | SDL.SDL_WindowFlags.SDL_WINDOW_POPUP_MENU
        );
        Renderer = SDL.SDL_CreateRenderer(Handle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        
        SDL.SDL_GetWindowPosition(Handle, out _x, out _y);
        var windowCount = Internal.Windows.Count;
        SDL.SDL_SetWindowPosition(Handle, _x + 5*windowCount, _y + 5*windowCount);
    }

    public virtual void Update(Clock clock)
    {
        
    }

    public virtual void Render()
    {
        /* HAVE THIS FUCKING STRUCTURE
        SDL.SDL_SetRenderDrawColor(Renderer, 100, 0, 0, 255);
        SDL.SDL_RenderClear(Renderer);
        
        //draw
        
        SDL.SDL_RenderPresent(Renderer);
        */ 
    }
}