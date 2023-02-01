using System.Drawing;
using SDL2;

namespace Engine;

public class Text
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
    public string Font { get; set; } = "Resources/Fonts/p5hatty.ttf";
    public int FontSize { get; set; } = 16;

    public SDL.SDL_Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            
            _font = SDL_ttf.TTF_OpenFont(Font, FontSize);
            if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());

            _textSurface = SDL_ttf.TTF_RenderText_Blended(_font, _textString, Color);
            if (_textSurface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());
        
            _textTexture = SDL.SDL_CreateTextureFromSurface(_renderer, _textSurface);
            if (_textTexture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());
        }
    }
    public Alignment Alignment { get; set; } = Alignment.Left;
    public bool IsVisible { get; set; } = true;

    public int PercentageVisible
    {
        get
        {
            return _percentVisible;
        }
        set
        {
            _percentVisible = value;
            int numChars = (int)(TextString.Length * PercentageVisible / 100);
            _textString = TextString.Substring(0, numChars);
        }
    }

    public string TextString
    {
        get
        {
            return _textString;
        }
        set
        {
            _textString = value;
            int numChars = (int)(TextString.Length * PercentageVisible / 100);
            _textString = TextString.Substring(0, numChars);
            
            _font = SDL_ttf.TTF_OpenFont(Font, FontSize);
            if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());

            _textSurface = SDL_ttf.TTF_RenderText_Blended(_font, _textString, Color);
            if (_textSurface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());
        
            _textTexture = SDL.SDL_CreateTextureFromSurface(_renderer, _textSurface);
            if (_textTexture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());
        }
    }

    private string _textString = "";
    private SDL.SDL_Color _color;
    private int _percentVisible = 100;
    private IntPtr _font;
    private IntPtr _textSurface;
    private IntPtr _textTexture;
    private IntPtr _renderer;

    public Text(string text, int x, int y, string font, int fontSize, IntPtr renderer)
    {
        _renderer = renderer;
        _font = SDL_ttf.TTF_OpenFont(Font, FontSize);
        if (_font == IntPtr.Zero) throw new Exception("Failed to load font: " + SDL_ttf.TTF_GetError());
        
        _textSurface = SDL_ttf.TTF_RenderText_Blended(_font, text, Color);
        if (_textSurface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());
        
        _textTexture = SDL.SDL_CreateTextureFromSurface(renderer, _textSurface);
        if (_textTexture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());
        TextString = text;
        X = x;
        Y = y;
        Font = font;
        FontSize = fontSize;
        Color = new SDL.SDL_Color()
        {
            r = 0,
            g = 0,
            b = 0,
            a = 255
        };
    }
    
    
    public void Draw()
    {
        if (!IsVisible) return;
        
        IntPtr textSurface = SDL_ttf.TTF_RenderText_Blended(_font, _textString, Color);
        if (textSurface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());
        
        IntPtr textTexture = SDL.SDL_CreateTextureFromSurface(_renderer, textSurface);
        if (textTexture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());
        
        int w, h;
        SDL.SDL_QueryTexture(textTexture, out _, out _, out w, out h);
        
        int x = X, y = Y;
        switch (Alignment)
        {
            case Alignment.Left:
                x = X;
                break;
            case Alignment.Center:
                x = X - w / 2;
                break;
            case Alignment.Right:
                x = X - w;
                break;
        }
        
        SDL.SDL_Rect destination = new SDL.SDL_Rect()
        {
            x = x,
            y = y,
            w = w,
            h = h
        };
        
        SDL.SDL_RenderCopy(_renderer, textTexture, IntPtr.Zero, ref destination);
        
        SDL.SDL_DestroyTexture(textTexture);
        SDL.SDL_FreeSurface(textSurface);
    }

    public void Draw(uint wrapWidth)
    {
        if (!IsVisible) return;
        
        IntPtr textSurface = SDL_ttf.TTF_RenderText_Blended_Wrapped(_font, _textString, Color, wrapWidth);
        if (textSurface == IntPtr.Zero) throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());
        
        IntPtr textTexture = SDL.SDL_CreateTextureFromSurface(_renderer, textSurface);
        if (textTexture == IntPtr.Zero) throw new Exception("Failed to create texture from surface: " + SDL.SDL_GetError());
        
        int w, h;
        SDL.SDL_QueryTexture(textTexture, out _, out _, out w, out h);
        
        int x = X, y = Y;
        switch (Alignment)
        {
            case Alignment.Left:
                x = X;
                break;
            case Alignment.Center:
                x = X - w / 2;
                break;
            case Alignment.Right:
                x = X - w;
                break;
        }
        
        SDL.SDL_Rect destination = new SDL.SDL_Rect()
        {
            x = x,
            y = y,
            w = w,
            h = h
        };
        
        SDL.SDL_RenderCopy(_renderer, textTexture, IntPtr.Zero, ref destination);
        
        SDL.SDL_DestroyTexture(textTexture);
        SDL.SDL_FreeSurface(textSurface);
    }


    
}