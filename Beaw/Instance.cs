using System;
using System.Collections.Generic;
using System.Numerics;
using Engine.Windows;
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
        private TextRenderer _textRenderer;

        public Instance(InstanceSettings instanceSettings)
        {
            _instanceSettings = instanceSettings;
        }

        public void Run()
        {
            SDL_ttf.TTF_Init();
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
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
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
            _dummyTexture = new("Resources/madeline.png", Internal.RendererHandle);
            //_dummyTexture.Rectangle.w /= 5;
            //_dummyTexture.Rectangle.h /= 5;
            _textRenderer = new(Internal.RendererHandle, "Resources/Fonts/arial.ttf", 26);
        }
        
        private void Update()
        {
            _clock.Update();
            foreach (var window in Internal.Windows)
            {
                window.Update(_clock);
            }
        }

        private void HandleEvents()
        {
            SDL.SDL_Event e;
            while (SDL.SDL_PollEvent(out e) == 1)
            {
                // handle multiple windows
                if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT
                    && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                {
                    for (int i = 0; i < Internal.Windows.Count;)
                    {
                        if (SDL.SDL_GetWindowID(Internal.Windows[i].Handle) == e.window.windowID)
                        {
                            SDL.SDL_DestroyWindow(Internal.Windows[i].Handle);
                            Internal.Windows.Remove(Internal.Windows[i]);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                // DEPRECTAED
                if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT
                    && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                {
                    foreach (var window in Internal.Windows)
                    {
                        if (SDL.SDL_GetWindowID(window.Handle) == e.window.windowID)
                        {
                            SDL.SDL_DestroyWindow(window.Handle);
                            Internal.Windows.Remove(window);
                        }
                    }
                }
                
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
                                Internal.Windows.Add(new ErrorWindow("Error", new(200, 100)));
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
            SDL.SDL_SetRenderDrawColor(Internal.RendererHandle, 88, 85, 83, 255);
            SDL.SDL_RenderClear(Internal.RendererHandle);

            // DRAW
            //_dummyTexture.Render(Internal.RendererHandle);
            _textRenderer.RenderTextWithWidth("The backlash against Russian culture in Ukraine had been picking up steam since 2014, when Russia occupied the Donbas and Crimea. But Russia’s unprovoked invasion of Ukraine, together with the horrors committed by its troops, has sent it into overdrive. De-Russification has mostly been a bottom-up process or a matter of individual preference, as opposed to government policy. Millions of Ukrainians continue to speak Russian without suffering discrimination. ", 50, 50, 500, 0, 0, 0, 255);
            foreach (var window in Internal.Windows)
            {
                window.Render();
            }
            
            
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

