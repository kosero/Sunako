using Raylib_cs;

namespace Sunako.Renderer.AnimatedSprite;

public class AnimatedSprite : Sprite.Sprite
{
    public SpriteFrames Frames { get; } = new();

    private string _currentAnim = "";
    private int _frameIndex = 0;
    private float _timer = 0f;

    public float Fps { get; set; } = 8f;
    public bool Loop { get; set; } = true;

    public void Play(string name)
    {
        if (_currentAnim == name) return;

        _currentAnim = name;
        _frameIndex = 0;
        _timer = 0f;
        IsDirty = true;
    }

    public void Update(float dt)
    {
        var anim = Frames.Get(_currentAnim);
        if (anim == null || anim.Count == 0) return;

        _timer += dt;
        var frameTime = 1f / Fps;

        if (!(_timer >= frameTime)) return;
        _timer -= frameTime;
        _frameIndex++;

        if (_frameIndex >= anim.Count)
        {
            if (Loop)
                _frameIndex = 0;
            else
                _frameIndex = anim.Count - 1;
        }

        IsDirty = true;
    }

    public override void UpdateTransform()
    {
        base.UpdateTransform();

        var anim = Frames.Get(_currentAnim);
        if (anim == null || anim.Count == 0) return;

        var rect = anim[_frameIndex];
        SourceRect = new Rectangle(
            rect.X,
            rect.Y,
            rect.Width * (FlipH ? -1 : 1),
            rect.Height * (FlipV ? -1 : 1)
        );

        var destWidth = MathF.Abs(rect.Width) * Scale;
        var destHeight = MathF.Abs(rect.Height) * Scale;

        DestRect = new Rectangle(
            MathF.Round(Position.X + Offset.X),
            MathF.Round(Position.Y + Offset.Y),
            destWidth,
            destHeight
        );

        OriginPoint = new System.Numerics.Vector2(
            destWidth * Origin.X,
            destHeight * Origin.Y
        );
    }

    public void ForceFrame(int index)
    {
        _frameIndex = index;
        _timer = 0f;
        IsDirty = true;
    }
}