using Raylib_cs;
using System.Numerics;

namespace Sunako.Renderer.Sprite;

public class Sprite
{
    private const float DefaultSize = 64.0f;

    // Identity
    public string Name { get; set; } = "";
    public string TexturePath { get; set; } = "";

    // Texture
    private Texture2D _texture;

    public Texture2D Texture
    {
        get => _texture;
        set => Set(ref _texture, value);
    }

    // Transform (local)
    private Vector2 _position = Vector2.Zero;
    private Vector2 _origin = new(0.5f, 0.5f);
    private Vector2 _offset = Vector2.Zero;
    private float _scale = 1.0f;
    private float _rotation;
    private int _zIndex;

    // Transform
    public Vector2 Position
    {
        get => _position;
        set => Set(ref _position, value);
    }

    public Vector2 Origin
    {
        get => _origin;
        set => Set(ref _origin, value);
    }

    public Vector2 Offset
    {
        get => _offset;
        set => Set(ref _offset, value);
    }

    public float Scale
    {
        get => _scale;
        set => Set(ref _scale, value);
    }

    public float Rotation
    {
        get => _rotation;
        set => Set(ref _rotation, value);
    }

    public int ZIndex
    {
        get => _zIndex;
        set => Set(ref _zIndex, value);
    }

    // State
    private bool _flipH = false;
    private bool _flipV = false;
    private bool _visible = true;
    private bool _active = false;
    private byte _alpha = 255;

    public bool FlipH
    {
        get => _flipH;
        set => Set(ref _flipH, value);
    }

    public bool FlipV
    {
        get => _flipV;
        set => Set(ref _flipV, value);
    }

    public bool Visible
    {
        get => _visible;
        set => Set(ref _visible, value);
    }

    public bool Active
    {
        get => _active;
        set => Set(ref _active, value);
    }

    public byte Alpha
    {
        get => _alpha;
        set => Set(ref _alpha, value);
    }

    public bool IsDirty { get; protected set; } = true;

    // Appearance
    public Color Tint { get; protected set; } = Color.White;
    public Color EmptyColor { get; set; } = Color.Magenta;

    // Cached
    public Rectangle SourceRect { get; set; }
    public Rectangle DestRect { get; protected set; }
    public Vector2 OriginPoint { get; protected set; }

    public Sprite()
    {
        Reset();
    }

    public void Reset()
    {
        Name = "";
        TexturePath = "";

        _texture = new Texture2D();

        _position = Vector2.Zero;
        _origin = new Vector2(0.5f, 0.5f);
        _offset = Vector2.Zero;
        _scale = 1.0f;
        _rotation = 0.0f;
        _zIndex = 0;

        _flipH = false;
        _flipV = false;
        _visible = true;
        _active = false;
        _alpha = 255;

        Tint = Color.White;
        EmptyColor = Color.Magenta;

        IsDirty = true;
    }

    private void Set<T>(ref T field, T value)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            IsDirty = true;
        }
    }

    public virtual void UpdateTransform()
    {
        var srcWidth = _texture.Id > 0 ? _texture.Width : DefaultSize;
        var srcHeight = _texture.Id > 0 ? _texture.Height : DefaultSize;

        SourceRect = new Rectangle(
            0,
            0,
            srcWidth * (_flipH ? -1 : 1),
            srcHeight * (_flipV ? -1 : 1)
        );

        var destWidth = srcWidth * _scale;
        var destHeight = srcHeight * _scale;

        DestRect = new Rectangle(
            _position.X + _offset.X,
            _position.Y + _offset.Y,
            MathF.Abs(destWidth),
            MathF.Abs(destHeight)
        );

        OriginPoint = new Vector2(
            destWidth * _origin.X,
            destHeight * _origin.Y
        );

        Tint = new Color((float)255, 255, 255, _alpha);
        IsDirty = false;
    }
}