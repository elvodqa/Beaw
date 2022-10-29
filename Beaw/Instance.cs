using System;
using System.Collections.Generic;
using System.Numerics;
using SDL2;

namespace Engine
{
    public struct InstanceSettings
    {
        public Vector2 Size;
        public string Title;
    }

    public struct Clock
    {
        private double _currentTime = 0;

        private ulong _now = SDL.SDL_GetPerformanceCounter();
        private ulong _lastTickTime = 0;
        public double Delta = 0;

        public Clock()
        {
        }

        public void Update()
        {
            _currentTime = SDL.SDL_GetTicks();

            _lastTickTime = _now;
            _now = SDL.SDL_GetPerformanceCounter();
            Delta = (_now - _lastTickTime) * 1000 / (double)SDL.SDL_GetPerformanceFrequency();
        }

        public void Restart()
        {
            _currentTime = 0;
        }

        public double AsMiliseconds()
        {
            return _currentTime;
        }

        public double AsSeconds()
        {
            return _currentTime / 1000;
        }
    }

    public class Instance : IDisposable
    {
        private InstanceSettings _instanceSettings;
        private bool _running = false;
        private double _startTime = SDL.SDL_GetTicks();
        private double _currentTime = SDL.SDL_GetTicks();
        private Clock _clock;
        private Texture _dummyTexture;

        public Instance(InstanceSettings instanceSettings)
        {
            _instanceSettings = instanceSettings;
        }

        public void Run()
        {
            _running = true;
            Load();
            while (_running)
            {
                Update();
                HandleEvents();
                Render();
            }
            
        }

        private void Load()
        {
            if (_instanceSettings.Size.X < 640)
            {
                Console.WriteLine("Given width is smaller than expected. It is set to 640.");
                _instanceSettings.Size.X = 640;
            }
            if (_instanceSettings.Size.Y < 480)
            {
                Console.WriteLine("Given height is smaller than expected. It is set to 480.");
                _instanceSettings.Size.Y = 480;
            }

            Internal.WindowHandle = SDL.SDL_CreateWindow(
                _instanceSettings.Title,
                100,
                100,
                (int)_instanceSettings.Size.X,
                (int)_instanceSettings.Size.Y,
                SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
            );

            if (Internal.WindowHandle == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
            }

            SDL.SDL_SetWindowMinimumSize(Internal.WindowHandle, 640, 480);

            Internal.RendererHandle = SDL.SDL_CreateRenderer(Internal.WindowHandle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (Internal.RendererHandle == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
            }

            _clock = new();
            _dummyTexture = new("Resources/emir.png");
        }
        
        private void Update()
        {
            _clock.Update();
            //Console.WriteLine(_clock.Delta);
        }

        private void HandleEvents()
        {
            SDL.SDL_Event e;
            while (SDL.SDL_PollEvent(out e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        _running = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_w)
                        {
                            SDL.SDL_Keymod modstates = SDL.SDL_GetModState();
                            if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            {
                                Console.WriteLine("Shift + W");
                            }
                            else
                            {
                                Console.WriteLine("W");
                            }

                        }
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_s)
                        {
                            SDL.SDL_Keymod modstates = SDL.SDL_GetModState();
                            if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            {
                                Console.WriteLine("Shift + S");
                            }
                            else
                            {
                                Console.WriteLine("s");
                            }

                        }
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a)
                        {
                            SDL.SDL_Keymod modstates = SDL.SDL_GetModState();
                            if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            {
                                Console.WriteLine("Shift + A");
                            }
                            else
                            {
                                Console.WriteLine("a");
                            }

                        }
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                        {
                            SDL.SDL_Keymod modstates = SDL.SDL_GetModState();
                            if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            {
                                Console.WriteLine("Shift + D");
                            }
                            else
                            {
                                Console.WriteLine("d");
                            }

                        }
                        break;
                }
            }

        }

        private void Render()
        {
            SDL.SDL_RenderClear(Internal.RendererHandle);

            // DRAW
            _dummyTexture.Render();

            SDL.SDL_RenderPresent(Internal.RendererHandle);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyWindow(Internal.WindowHandle);
            SDL.SDL_DestroyRenderer(Internal.RendererHandle);
            SDL.SDL_Quit();
        }
    }
}

