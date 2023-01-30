using System.Runtime.InteropServices;
using SDL2;

namespace Engine;

public class Texture2D
{
    public IntPtr Texture { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Texture2D(string fileName, IntPtr renderer)
    {
        IntPtr surface = SDL.SDL_LoadBMP(fileName);
        Texture = SDL_image.IMG_LoadTexture(renderer, fileName);
        if (Texture == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating a texture: {SDL.SDL_GetError()}");
        }
        SDL.SDL_FreeSurface(surface);

        int width, height;
        SDL.SDL_QueryTexture(Texture, out uint format, out int access, out width, out height);
        Width = width;
        Height = height;
    }
}

public class SpriteBatch
{
    private IntPtr _renderer;
    private IntPtr _font;
    private int _fontSize = 16;
    private string _fontPath = "Resources/Fonts/p5hatty.ttf";

    public int FontSize
    {
        get
        {
            return _fontSize;
        }
        set
        {
            _fontSize = value;
            _font = SDL_ttf.TTF_OpenFont(_fontPath, value);
            if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());
        }
    }
    
    public string FontPath
    {
        get
        {
            return _fontPath;
        }
        set
        {
            _fontPath = value;
            _font = SDL_ttf.TTF_OpenFont(value, _fontSize);
            if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());
        }
    }

    public SpriteBatch(IntPtr renderer)
    {
        _renderer = renderer;
        _font = SDL_ttf.TTF_OpenFont(_fontPath, _fontSize);
        if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());
    }

    public void Begin()
    {
        SDL.SDL_RenderClear(_renderer);
    }

    public void End()
    {
        SDL.SDL_RenderPresent(_renderer);
    }

    public void Draw(Texture2D texture, int x, int y, int width, int height, float scaleX, float scaleY, float rotation)
    {
        SDL.SDL_Rect dst = new SDL.SDL_Rect
        {
            x = x,
            y = y,
            w = (int)(width * scaleX),
            h = (int)(height * scaleY),
        };
        SDL.SDL_RenderCopyEx(_renderer, texture.Texture, IntPtr.Zero, ref dst, rotation, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
    }
    
    public void DrawText(string text, int x, int y, byte r, byte g, byte b, byte a)
    {
        var color = new SDL.SDL_Color
        {
            r = r,
            g = g,
            b = b,
            a = a
        };

        var surface = SDL_ttf.TTF_RenderUTF8_Blended(_font, text, color);
        if (surface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());

        var texture = SDL.SDL_CreateTextureFromSurface(_renderer, surface);
        if (texture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());

        var _surface = (SDL.SDL_Surface)(Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface)) ?? throw new InvalidOperationException());
        var dstRect = new SDL.SDL_Rect
        {
            x = x,
            y = y,
            w = _surface.w,
            h = _surface.h
        };

        SDL.SDL_RenderCopy(_renderer, texture, IntPtr.Zero, ref dstRect);
        SDL.SDL_DestroyTexture(texture);
        SDL.SDL_FreeSurface(surface);
    }
    
    public void DrawTextWithWidth(string text, int x, int y, uint width, byte r, byte g, byte b, byte a)
    {
        var color = new SDL.SDL_Color
        {
            r = r,
            g = g,
            b = b,
            a = a
        };

        IntPtr surface = SDL_ttf.TTF_RenderText_Blended_Wrapped(_font, text, color, width);
        IntPtr texture = SDL2.SDL.SDL_CreateTextureFromSurface(_renderer, surface);
        SDL2.SDL.SDL_Rect dst = new SDL2.SDL.SDL_Rect
        {
            x = x,
            y = y,
            w = (int)width,
            h = 0,
        };
        SDL2.SDL.SDL_QueryTexture(texture, out uint format, out int access, out dst.w, out dst.h);
        SDL2.SDL.SDL_RenderCopy(_renderer, texture, IntPtr.Zero, ref dst);
        SDL2.SDL.SDL_FreeSurface(surface);
        SDL2.SDL.SDL_DestroyTexture(texture);
    }

}