using System.Runtime.InteropServices;
using SDL2;

namespace Engine;

[Obsolete("Use SpriteBatch instead.")]
public class TextRenderer
{
    private readonly IntPtr _font;
    private readonly IntPtr _renderer;

    public TextRenderer(IntPtr renderer, string fontPath, int fontSize)
    {
        _renderer = renderer;
        _font = SDL_ttf.TTF_OpenFont(fontPath, fontSize);
        if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());
    }
    
    [Obsolete("Use SpriteBatch.DrawText instead.")]
    public void RenderText(string text, int x, int y, byte r, byte g, byte b, byte a)
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
    
    [Obsolete("Use SpriteBatch.DrawTextWithWidth instead.")]
    public void RenderTextWithWidth(string text, int x, int y, uint width, byte r, byte g, byte b, byte a)
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




    [Obsolete("Use RenderTextWithWidth instead", true)]
    public void RenderTextWithWidthOld(string text, int x, int y, int width, byte r, byte g, byte b, byte a)
    {
        var color = new SDL.SDL_Color
        {
            r = r,
            g = g,
            b = b,
            a = a
        };

        // Split the text into lines that fit within the specified width
        var lines = SplitTextIntoLines(text, width);

        var lineHeight = SDL_ttf.TTF_FontLineSkip(_font);
        var currentY = y;
        foreach (var line in lines)
        {
            var surface = SDL_ttf.TTF_RenderUTF8_Blended(_font, line, color);
            if (surface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());

            var texture = SDL.SDL_CreateTextureFromSurface(_renderer, surface);
            if (texture == IntPtr.Zero)
                throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());

            var _surface = (SDL.SDL_Surface)(Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface)) ?? throw new InvalidOperationException());
            var dstRect = new SDL.SDL_Rect
            {
                x = x,
                y = currentY,
                w = _surface.w,
                h = _surface.h
            };

            SDL.SDL_RenderCopy(_renderer, texture, IntPtr.Zero, ref dstRect);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surface);

            currentY += lineHeight;
        }
    }

    private string[] SplitTextIntoLines(string text, int width)
    {
        var lines = new List<string>();

        var startIndex = 0;
        var currentIndex = 0;
        while (currentIndex < text.Length)
        {
            var endIndex = currentIndex + 1;
            while (endIndex < text.Length)
            {
                var subString = text.Substring(startIndex, endIndex - startIndex + 1);
                int textWidth;
                SDL_ttf.TTF_SizeUTF8(_font, subString, out textWidth, out _);
                if (textWidth > width)
                {
                    if (endIndex > startIndex + 1) endIndex--;
                    break;
                }

                endIndex++;
            }

            var nextIndex = text.IndexOf(" ", endIndex);
            if (nextIndex == -1) nextIndex = text.Length;

            var line = text.Substring(startIndex, nextIndex - startIndex).Trim();
            lines.Add(line);

            startIndex = nextIndex + 1;
            currentIndex = nextIndex + 1;
        }

        return lines.ToArray();
    }
}