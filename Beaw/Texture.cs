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


        public Texture(string path)
        {
            TextureHandle = SDL_image.IMG_LoadTexture(Internal.RendererHandle, path);
            Rectangle.x = 0;
            Rectangle.y = 0;
            SDL.SDL_QueryTexture(TextureHandle, out _format, out _access, out Rectangle.w, out Rectangle.h);
            Rectangle.w = 100;
            Rectangle.h = 100;
        }

        public void Update(Clock clock)
        {
            
        }

        public void Render()
        {
            SDL.SDL_RenderCopy(Internal.RendererHandle, TextureHandle, IntPtr.Zero, ref Rectangle);
        }

        public void Dispose()
        {
            
        }
    }
}

