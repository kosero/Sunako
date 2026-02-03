namespace Sunako.Renderer.Sprite;

[Flags]
public enum SpriteDebugFlags
{
    None = 0,
    Box = 1 << 0,
    Origin = 1 << 1,
    Name = 1 << 2,
    All = 0xFF
}