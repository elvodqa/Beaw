using System.Drawing;

namespace Engine.UI;

public class Button : UIElement
{
    public int Width, Height;
    public string Text;
    private Text _text;
    private Rectangle _rectangle;
    private RectangleF _boundingBox;
    // add on press event handler
    public event EventHandler<EventArgs> Pressed;
    private int _x, _y = 0;
    public int X
    {
        get
        {
            return _x;
        }
        set
        {
            _x = value;
            _rectangle = new Rectangle(_x, _y, Width, Height, Color.FromArgb(255, 255, 255, 255));
            var __x = _rectangle.X + _rectangle.Width / 2;
            var __y = _rectangle.Y + _rectangle.Height / 3;
            _text = new Text(Text, __x, __y, "Resources/Fonts/p5hatty.ttf", 24, Layer.Renderer);
            _text.Alignment = Alignment.Center;
            _boundingBox = new RectangleF(_rectangle.X, _rectangle.Y, _rectangle.Width, _rectangle.Height);
        }
    }
    public int Y
    {
        get
        {
            return _y;
        }
        set
        {
            _y = value;
            _rectangle = new Rectangle(_x, _y, Width, Height, Color.FromArgb(255, 255, 255, 255));
            var __x = _rectangle.X + _rectangle.Width / 2;
            var __y = _rectangle.Y + _rectangle.Height / 3;
            _text = new Text(Text, __x, __y, "Resources/Fonts/p5hatty.ttf", 24, Layer.Renderer);
            _text.Alignment = Alignment.Center;
            _boundingBox = new RectangleF(_rectangle.X, _rectangle.Y, _rectangle.Width, _rectangle.Height);
        }
    }

    

    public Button(string text, int x, int y, int width, int height, Layer layer)
    {
        Width = width;
        Height = height;
        Text = text;
        Layer = layer;
        X = x;
        Y = y;
        layer.AddElement(this);
        _rectangle = new Rectangle(x, y, width, height, Color.FromArgb(255, 255, 255, 255));
        var _x = _rectangle.X + _rectangle.Width / 2;
        var _y = _rectangle.Y + _rectangle.Height / 3;
        _text = new Text(text, _x, _y, "Resources/Fonts/p5hatty.ttf", 24, layer.Renderer);
        _text.Alignment = Alignment.Center;
        _boundingBox = new RectangleF(_rectangle.X, _rectangle.Y, _rectangle.Width, _rectangle.Height);

    }  
    public override void Update(Clock clock)
    {
        base.Update(clock);
        if (_boundingBox
            .Contains(Layer.Instance.Input.GetMousePosition().x, Layer.Instance.Input.GetMousePosition().y))
        {
            _rectangle.Color = Color.FromArgb(255, 88, 77, 255);
            if (Layer.Instance.Input.IsMouseJustDown(0))
            {
                Console.WriteLine("Pressed");
                OnPressed();
            }
        }
        else
        {
            _rectangle.Color = Color.FromArgb(255, 70, 100, 100);
        } 
    }
    
    public override void Render(IntPtr renderer)
    {
        base.Render(renderer);
        _rectangle.Render(renderer);
        _text.Draw();
    }
    
    protected virtual void OnPressed()
    {
        Pressed?.Invoke(this, EventArgs.Empty);
    }
}