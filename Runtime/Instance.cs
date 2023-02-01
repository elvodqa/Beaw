using System.Numerics;
using Engine;
using Engine.Windows;
using SDL2;

namespace Runtime;

internal class Instance : IDisposable
{
    private Clock _clock;
    private double _currentTime = SDL.SDL_GetTicks();
    private Texture2D _dummyTexture;
    public InstanceSettings InstanceSettings;
    private string _randomtext = "";
    private readonly int _rotation = 0;
    private bool _running;
    private double _startTime = SDL.SDL_GetTicks();
    private TextRenderer _textRenderer;
    private Input Input;

    public Instance(InstanceSettings instanceSettings)
    {
        InstanceSettings = instanceSettings;
    }

    public LuaAPI LuaApi { get; set; } = new();

    public void Dispose()
    {
        SDL.SDL_DestroyWindow(Internal.WindowHandle);
        SDL.SDL_DestroyRenderer(Internal.RendererHandle);
        SDL.SDL_Quit();
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
        if (InstanceSettings.Size.X < 640)
        {
            Console.WriteLine("Given width is smaller than expected. It is set to 640.");
            InstanceSettings.Size.X = 640;
        }

        if (InstanceSettings.Size.Y < 480)
        {
            Console.WriteLine("Given height is smaller than expected. It is set to 480.");
            InstanceSettings.Size.Y = 480;
        }

        Internal.WindowHandle = SDL.SDL_CreateWindow(
            InstanceSettings.Title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            (int)InstanceSettings.Size.X,
            (int)InstanceSettings.Size.Y,
            SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
        );

        if (Internal.WindowHandle == nint.Zero)
            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");

        SDL.SDL_SetWindowMinimumSize(Internal.WindowHandle, 640, 480);

        Internal.RendererHandle =
            SDL.SDL_CreateRenderer(Internal.WindowHandle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        if (Internal.RendererHandle == nint.Zero)
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");

        _clock = new Clock();
        _dummyTexture = new Texture2D("Resources/madeline.png", Internal.RendererHandle);


        _textRenderer = new TextRenderer(Internal.RendererHandle, "Resources/Fonts/p5hatty.ttf", 26);
        Input = new Input();
        Internal.SpriteBatch = new SpriteBatch(Internal.RendererHandle);
        Internal.SpriteBatch.FontSize = 26;
    }

    private void Update()
    {
        _clock.Update();
        foreach (var window in Internal.Windows) window.Update(_clock);
        Input.Update();

        if (Input.IsKeyJustDown(SDL.SDL_Scancode.SDL_SCANCODE_A)) Console.WriteLine("A");

        _randomtext += Input.GetPressedChar();
    }

    private void HandleEvents()
    {
        SDL.SDL_Event e;
        while (SDL.SDL_PollEvent(out e) == 1)
        {
            // handle multiple windows
            if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT
                && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                for (var i = 0; i < Internal.Windows.Count;)
                    if (SDL.SDL_GetWindowID(Internal.Windows[i].Handle) == e.window.windowID)
                    {
                        SDL.SDL_DestroyWindow(Internal.Windows[i].Handle);
                        Internal.Windows.Remove(Internal.Windows[i]);
                    }
                    else
                    {
                        i++;
                    }

            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) Input.UpdateEvent(e);
            // DEPRECTAED
            if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT
                && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                foreach (var window in Internal.Windows)
                    if (SDL.SDL_GetWindowID(window.Handle) == e.window.windowID)
                    {
                        SDL.SDL_DestroyWindow(window.Handle);
                        Internal.Windows.Remove(window);
                    }

            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    _running = false;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_w)
                    {
                        var modstates = SDL.SDL_GetModState();
                        if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            Console.WriteLine("Shift + W");
                        else
                            Console.WriteLine("W");
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_s)
                    {
                        var modstates = SDL.SDL_GetModState();
                        if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            Console.WriteLine("Shift + S");
                        else
                            Console.WriteLine("s");
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a)
                    {
                        var modstates = SDL.SDL_GetModState();
                        if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                        {
                            Console.WriteLine("Shift + A");
                            Internal.Windows.Add(new ErrorWindow("Error", new Vector2(200, 100)));
                        }
                        else
                        {
                            Console.WriteLine("a");
                        }
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                    {
                        var modstates = SDL.SDL_GetModState();
                        if (modstates.HasFlag(SDL.SDL_Keymod.KMOD_LSHIFT))
                            Console.WriteLine("Shift + D");
                        else
                            Console.WriteLine("d");
                    }

                    break;
            }
        }
    }

    private void Render()
    {
        SDL.SDL_SetRenderDrawColor(Internal.RendererHandle, 88, 85, 83, 255);
        SDL.SDL_RenderClear(Internal.RendererHandle);


        Internal.SpriteBatch.Draw(_dummyTexture, 100, 100, 200, 200, 1, 1, _rotation);


        //_textRenderer.RenderTextWithWidth("The backlash against Russian culture in Ukraine had been picking up steam since 2014, when Russia occupied the Donbas and Crimea. But Russia’s unprovoked invasion of Ukraine, together with the horrors committed by its troops, has sent it into overdrive. De-Russification has mostly been a bottom-up process or a matter of individual preference, as opposed to government policy. Millions of Ukrainians continue to speak Russian without suffering discrimination. ", 50, 50, 500, 0, 0, 0, 255);
        Internal.SpriteBatch.DrawText("Expected: * ’ ' - , . ! ? [ ] { }", 50, 20, 0, 0, 0, 255);
        Internal.SpriteBatch.DrawTextWithWidth("Result: * ’ ' - , . ! ? [ ] { }", 50, 50, 500, 0, 0, 0, 255);
        Internal.SpriteBatch.DrawText("_randomtext", 0, 70, 255, 255, 255, 255);
        foreach (var window in Internal.Windows) window.Render();


        SDL.SDL_RenderPresent(Internal.RendererHandle);
    }
}