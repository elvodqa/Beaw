using System.Numerics;
using Engine;
using Engine.UI;
using Engine.Windows;
using SDL2;

namespace Runtime;

internal enum InstanceState
{
    OnMenu,
    OnPause,
    OnGame
}

public class Instance : IDisposable
{
    private readonly int _rotation = 0;
    private Clock _clock;
    private double _currentTime = SDL.SDL_GetTicks();
    private Texture2D _dummyTexture;
    private readonly InstanceState _instanceState = InstanceState.OnMenu;
    private string _randomtext = "";
    private bool _running;
    private double _startTime = SDL.SDL_GetTicks();
    private TextRenderer _textRenderer;
    public string GameTitle;
    public Input Input;
    private Layer _menuLayer;
    private Button _startButton;
    private Button _loadButton;
    private Button _optionsButton;
    private Button _aboutButton;
    private Button _quitButton;
    public InstanceSettings InstanceSettings;
    public int WindowWidth, WindowHeight;

    public Instance(InstanceSettings instanceSettings)
    {
        InstanceSettings = instanceSettings;
    }

    public LuaAPI LuaApi { get; set; } = new();
    public Text TitleText { get; set; }

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
        WindowWidth = (int)InstanceSettings.Size.X;
        WindowHeight = (int)InstanceSettings.Size.Y;
        while (_running)
        {
            Update();
            HandleEvents();
            Render();
        }
    }

    private void Load()
    {
        #region Window

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
      
        SDL.SDL_GL_SetSwapInterval(1);

    // nint.Zero
         if (Internal.WindowHandle == nint.Zero)
            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");

        SDL.SDL_SetWindowMinimumSize(Internal.WindowHandle, 640, 480);

        Internal.RendererHandle =
            SDL.SDL_CreateRenderer(Internal.WindowHandle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        if (Internal.RendererHandle == nint.Zero)
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");

        #endregion

        _clock = new Clock();
        _dummyTexture = new Texture2D("Resources/madeline.png", Internal.RendererHandle);


        _textRenderer = new TextRenderer(Internal.RendererHandle, "Resources/Fonts/p5hatty.ttf", 26);
        Input = new Input();
        Internal.SpriteBatch = new SpriteBatch(Internal.RendererHandle);
        Internal.SpriteBatch.FontSize = 26;

        TitleText = new Text(GameTitle, 10, (int)(InstanceSettings.Size.Y- 360), "Resources/Fonts/p5hatty.ttf", 26,
            Internal.RendererHandle);
        TitleText.FontSize = 40;
        TitleText.Alignment = Alignment.Left;
        TitleText.Color = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };

        _menuLayer = new(Internal.RendererHandle, this);
        _startButton = new("Start", 10, (int)(InstanceSettings.Size.Y- 300), 100, 50, _menuLayer);
        _loadButton = new("Load", 10, (int)(InstanceSettings.Size.Y- 240), 100, 50, _menuLayer);
        _optionsButton = new("Options", 10, (int)(InstanceSettings.Size.Y- 180), 100, 50, _menuLayer);
        _aboutButton = new("About", 10, (int)(InstanceSettings.Size.Y- 120), 100, 50, _menuLayer);
        _quitButton = new("Quit", 10, (int)(InstanceSettings.Size.Y- 60), 100, 50, _menuLayer);
        
        
        _quitButton.Pressed += (sender, args) =>
        {
            _running = false;
            SDL.SDL_Quit();
        };
    }

    private void Update()
    {
        _clock.Update();
        foreach (var window in Internal.Windows) window.Update(_clock);
        

        switch (_instanceState)
        {
            case InstanceState.OnMenu:
            {
                _menuLayer.Update(_clock);
                break;
            }
            case InstanceState.OnGame:
            {
                break;
            }
        }
        Input.Update();
    }

    private void HandleEvents()
    {
        SDL.SDL_Event e;
        if (SDL.SDL_WaitEvent(out e) == 1)
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

            
            Input.UpdateEvent(e);
            // if window resized 
            if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT
                && e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
            {
                WindowWidth = e.window.data1;
                WindowHeight = e.window.data2;
                TitleText.Y = (WindowHeight - 360);
                _startButton.Y = (WindowHeight - 300);
                _loadButton.Y = (WindowHeight - 240);
                _optionsButton.Y = (WindowHeight - 180);
                _aboutButton.Y = (WindowHeight - 120);
                _quitButton.Y = (WindowHeight - 60);
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


        //Internal.SpriteBatch.Draw(_dummyTexture, 100, 100, 200, 200, 1, 1, _rotation);


        switch (_instanceState)
        {
            case InstanceState.OnMenu:
            {
                TitleText.Draw();
                _menuLayer.Render(Internal.RendererHandle);
                break;
            }
            case InstanceState.OnGame:
            {
                break;
            }
        }

        foreach (var window in Internal.Windows) window.Render();


        SDL.SDL_RenderPresent(Internal.RendererHandle);
    }
}