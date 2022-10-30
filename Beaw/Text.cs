using SDL2;

namespace Engine;

public class Text : IRenderable
{
    public IntPtr Handle { get; set; }
    public string Font { get; set; }
    public string TextString { get; set; }
    public uint Size { get; set; } = 24;
    public SDL.SDL_Color Color { get; set; } = new SDL.SDL_Color();

    private IntPtr surfaceMessage;
    private IntPtr MessageHandle;
    private SDL.SDL_Rect MessageRect;
    private IntPtr renderer;

    // CharisSIL-Regular.ttf
    // arial.ttf
    public Text( IntPtr renderer, string text, uint size = 10 ,string font = "arial.ttf")
    {
        this.renderer = renderer;
        var color = Color;
        color.r = 255;
        color.g = 255;
        color.b = 255;
        Color = color;
        TextString = text;
        Font = font;
        Size = size;

        Console.WriteLine($"Selected font: {Font}, Loading from: Resources/Fonts/{Font}");

        if (SDL_ttf.TTF_Init() == -1)
        {
            Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL_ttf.TTF_GetError());
            return;
        }

        Handle = SDL_ttf.TTF_OpenFont("Resources/Fonts/" + Font, (int)Size * 2);
        surfaceMessage = SDL_ttf.TTF_RenderText_Blended_Wrapped(Handle, TextString, Color, 750);
        MessageHandle = SDL.SDL_CreateTextureFromSurface(this.renderer, surfaceMessage);
        
        MessageRect.x = 0;
        MessageRect.y = 0;
        MessageRect.w = 750;
        //(int)Size * TextString.Length / 3; 
        MessageRect.h = (int)Size * TextString.Length / 70 * 5; 
    }
    
    public void Render(IntPtr renderer)
    {
        SDL.SDL_RenderCopy(renderer, MessageHandle, IntPtr.Zero, ref MessageRect);
    }
    
    public void Dispose()
    {
    }
}