using Runtime;

namespace Engine.UI;

public class Layer : IUpdatable, IRenderable
{
    private bool _isVisible = true;

    public List<UIElement> Elements = new();
    public IntPtr Renderer { get; set; }
    public Instance Instance { get; set; }

    public bool IsVisible
    {
        get { return _isVisible; }
        set
        {
            _isVisible = value;
            foreach (var element in Elements)
            {
                element.IsVisible = value;
            }
        }
    }

    public Layer(IntPtr renderer, Instance instance)
    {
        Renderer = renderer;
        Instance = instance;
    }

    public void AddElement(UIElement element)
    {
        Elements.Add(element);
    }

    public void Update(Clock clock)
    {
        foreach (var element in Elements)
        {
            element.Update(clock);
        }
    }

    public void Render(IntPtr renderer)
    {
        foreach (var element in Elements)
        {
            element.Render(renderer);
        }
    }

    public void Dispose()
    {
    }
}