using System;
namespace Engine
{
    public interface IRenderable : IDisposable
    {
        public void Render();
    }
}

