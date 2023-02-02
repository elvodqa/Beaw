using System.Drawing;
using SDL2;

namespace Engine;

public class Rectangle
{
    // make a drawable rectangle with outline and stuff using SDL2
    public int X, Y, Width, Height;
    public Color Color;
    public bool IsVisible = true;
    
    public Rectangle(int x, int y, int width, int height, Color color)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
    }
    
    public void Render(IntPtr renderer)
    {
        if (!IsVisible) return;
        SDL.SDL_SetRenderDrawColor(renderer, Color.R, Color.G, Color.B, Color.A);
        SDL.SDL_Rect rect = new SDL.SDL_Rect();
        rect.x = X;
        rect.y = Y;
        rect.w = Width;
        rect.h = Height;
        SDL.SDL_RenderFillRect(renderer, ref rect);
    }
    
    public void Dispose()
    {
        
    }

}