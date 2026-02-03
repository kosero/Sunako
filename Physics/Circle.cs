using System.Numerics;
using Raylib_cs;

namespace Sunako.Physics;

public class Circle
{
    public Vector2 Position;
    public float Radius;

    public Circle(Vector2 position, float radius)
    {
        Position = position;
        Radius = radius;
    }

    public Circle(float x, float y, float radius)
    {
        Position = new Vector2(x, y);
        Radius = radius;
    }

    public void Draw(Color color)
    {
        Raylib.DrawCircleV(Position, Radius, color);
    }

    public void DrawLines(Color color)
    {
        Raylib.DrawCircleLines((int)Position.X, (int)Position.Y, Radius, color);
    }
}