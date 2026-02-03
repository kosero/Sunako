using System.Numerics;
using Raylib_cs;

namespace Sunako.Physics;

public struct Aabb
{
    public Vector2 Position;
    public Vector2 Size;

    public float X
    {
        get => Position.X;
        set => Position.X = value;
    }

    public float Y
    {
        get => Position.Y;
        set => Position.Y = value;
    }

    public float Width
    {
        get => Size.X;
        set => Size.X = value;
    }

    public float Height
    {
        get => Size.Y;
        set => Size.Y = value;
    }

    public Vector2 Center => Position + Size * 0.5f;

    public float Left => Position.X;
    public float Right => Position.X + Size.X;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Size.Y;

    public Aabb(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }

    public Aabb(float x, float y, float width, float height)
    {
        Position = new Vector2(x, y);
        Size = new Vector2(width, height);
    }

    public void Draw(Color color)
    {
        Raylib.DrawRectangleV(Position, Size, color);
    }

    public void DrawLines(Color color)
    {
        Raylib.DrawRectangleLinesEx(new Rectangle(Position.X, Position.Y, Size.X, Size.Y), 1.0f, color);
    }
}
