namespace Engine.UI;

public class UIElement : IRenderable, IUpdatable
{
    public int X, Y;
    public Layer Layer;
    public bool IsVisible = true;

    public void Dispose()
    {
        
    }

    public virtual void Render(nint renderer)
    {
        if (!IsVisible) return;
    }

    public virtual void Update(Clock clock)
    {
        if (!IsVisible) return;
    }
}