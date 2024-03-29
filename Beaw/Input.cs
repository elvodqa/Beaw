﻿using System.Runtime.InteropServices;
using SDL2;

namespace Engine;

public class Input
{
    private bool[] _keyStates;
    private bool[] _keyDownThisFrame;
    private bool[] _mouseStates;
    private bool[] _mouseDownThisFrame;

    public Input()
    {
        _keyStates = new bool[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
        _mouseStates = new bool[3];
        _mouseDownThisFrame = new bool[3];
        _keyDownThisFrame = new bool[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
    }

    public void Update()
    {
        Array.Clear(_mouseDownThisFrame, 0, _mouseDownThisFrame.Length);
        Array.Clear(_keyDownThisFrame, 0, _keyDownThisFrame.Length);
        var keyboardState = SDL.SDL_GetKeyboardState(out int numKeys);
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardState, keys, 0, numKeys);
        for (int i = 0; i < numKeys; i++)
        {
            _keyStates[i] = keys[i] == 1;
        }

        var mouseState = SDL.SDL_GetMouseState(out int x, out int y);
        _mouseStates[0] = (mouseState & SDL.SDL_BUTTON(SDL.SDL_BUTTON_LEFT)) != 0;
        _mouseStates[1] = (mouseState & SDL.SDL_BUTTON(SDL.SDL_BUTTON_MIDDLE)) != 0;
        _mouseStates[2] = (mouseState & SDL.SDL_BUTTON(SDL.SDL_BUTTON_RIGHT)) != 0;
    }

    public void UpdateEvent(SDL.SDL_Event e)
    {
        if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            _keyDownThisFrame[(int)e.key.keysym.scancode] = true;
        }
        if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            Console.WriteLine("[Mouse Down]");
            if (e.button.button == SDL.SDL_BUTTON_LEFT)
            {
                _mouseDownThisFrame[0] = true;
            }
            if (e.button.button == SDL.SDL_BUTTON_MIDDLE)
            {
                _mouseDownThisFrame[1] = true;
            }
            if (e.button.button == SDL.SDL_BUTTON_RIGHT)
            {
                _mouseDownThisFrame[2] = true;
            }
        }
    
    }

    public bool IsKeyDown(SDL.SDL_Scancode scancode)
    {
        return _keyStates[(int)scancode];
    }

    public bool IsKeyJustDown(SDL.SDL_Scancode scancode)
    {
        return _keyDownThisFrame[(int)scancode];
    }

    public bool IsMouseDown(int button)
    {
        return _mouseStates[button];
    }
    
    
    public bool IsMouseJustDown(int button)
    {
        return _mouseDownThisFrame[button];
    }

    public (int x, int y) GetMousePosition()
    {
        SDL.SDL_GetMouseState(out int x, out int y);
        return (x, y);
    }
    
    public char GetPressedChar()
    {
        //var keyboardState = SDL.SDL_GetKeyboardState(out int numKeys);
        int numKeys;
        IntPtr keyboardState = SDL.SDL_GetKeyboardState(out numKeys);
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardState, keys, 0, numKeys);

        for (int i = 0; i < numKeys; i++)
        {
            if (keys[i] == 1)
            {
                SDL.SDL_Keycode key = SDL.SDL_GetKeyFromScancode((SDL.SDL_Scancode)i);
                char c = (char)key;
                if (char.IsLetterOrDigit(c))
                {
                    return c;
                }
            }
        }

        return '\0';
    }
}