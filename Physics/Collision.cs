using System.Numerics;

namespace Sunako.Physics;

public static class Collision
{
    // AABB vs AABB
    public static bool Check(Aabb a, Aabb b)
    {
        return a.Right >= b.Left && a.Left <= b.Right &&
               a.Bottom >= b.Top && a.Top <= b.Bottom;
    }

    // Circle vs Circle
    public static bool Check(Circle a, Circle b)
    {
        var dx = a.Position.X - b.Position.X;
        var dy = a.Position.Y - b.Position.Y;
        var distanceSquared = dx * dx + dy * dy;
        var radiusSum = a.Radius + b.Radius;
        return distanceSquared <= radiusSum * radiusSum;
    }

    // AABB vs Circle
    public static bool Check(Aabb box, Circle circle)
    {
        var closestX = Math.Clamp(circle.Position.X, box.Left, box.Right);
        var closestY = Math.Clamp(circle.Position.Y, box.Top, box.Bottom);

        var dx = circle.Position.X - closestX;
        var dy = circle.Position.Y - closestY;

        var distanceSquared = dx * dx + dy * dy;
        return distanceSquared <= circle.Radius * circle.Radius;
    }

    public static bool Check(Circle circle, Aabb box)
    {
        return Check(box, circle);
    }

    // Point vs AABB
    public static bool Check(Vector2 point, Aabb box)
    {
        return point.X >= box.Left && point.X <= box.Right &&
               point.Y >= box.Top && point.Y <= box.Bottom;
    }

    // Point vs Circle
    public static bool Check(Vector2 point, Circle circle)
    {
        var dx = point.X - circle.Position.X;
        var dy = point.Y - circle.Position.Y;
        return dx * dx + dy * dy <= circle.Radius * circle.Radius;
    }

    private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
    }
}
