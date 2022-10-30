using System;
using SDL2;

namespace Engine
{
    public class Texture : IRenderable, IUpdatable
    {
        public IntPtr TextureHandle;
        public SDL.SDL_Rect Rectangle;
        private uint _format;
        private int _access;


        public Texture(string path, IntPtr renderer)
        {
            TextureHandle = SDL_image.IMG_LoadTexture(renderer, path);
            if (TextureHandle == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating a texture: {SDL.SDL_GetError()}");
            }
            Rectangle.x = 0;
            Rectangle.y = 0;
            SDL.SDL_QueryTexture(TextureHandle, out _format, out _access, out Rectangle.w, out Rectangle.h);
        }

        public void Update(Clock clock)
        {
            
        }

        public void Render(IntPtr renderer)
        {
            SDL.SDL_RenderCopy(renderer, TextureHandle, IntPtr.Zero, ref Rectangle);
        }

        public void Dispose()
        {
            
        }
    }
}

