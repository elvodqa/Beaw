using Engine;

namespace Runtime;

internal static class Internal
{
    public static IntPtr WindowHandle;
    public static IntPtr RendererHandle;
    public static SpriteBatch SpriteBatch;
    public static List<Window> Windows = new();
}