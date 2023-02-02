using Engine;

namespace Runtime;

internal static class Internal
{
    public static nint WindowHandle;
    public static nint RendererHandle;
    public static SpriteBatch SpriteBatch;
    public static List<Window> Windows = new();
}