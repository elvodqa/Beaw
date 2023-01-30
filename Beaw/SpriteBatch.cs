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

    public SpriteBatch(IntPtr renderer)
    {
        _renderer = renderer;
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
}