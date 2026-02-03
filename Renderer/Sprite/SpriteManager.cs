using Raylib_cs;

namespace Sunako.Renderer.Sprite;

public static class SpriteManager
{
    private static readonly List<Sprite> Pool = new();
    private static bool _needsSort = true;

    private static SpriteDebugFlags _debugFlags = SpriteDebugFlags.None;
    private static readonly Color DebugColorBox = Color.Green;
    private static readonly Color DebugColorOrigin = Color.Red;

    public static void InitPool()
    {
        Pool.Clear();
        _needsSort = true;
    }

    public static Sprite AddSprite(Texture2D texture, string name)
    {
        var s = new Sprite();
        s.Reset();
        s.Name = name;
        s.Texture = texture;
        s.Active = true;
        s.Visible = true;

        Pool.Add(s);
        _needsSort = true;
        return s;
    }

    public static void Destroy(Sprite? sprite)
    {
        if (sprite is not { Active: true })
            return;

        if (sprite.Texture.Id > 0)
            Raylib.UnloadTexture(sprite.Texture);

        sprite.Active = false;
        sprite.Visible = false;

        Pool.Remove(sprite);
    }

    public static void DestroyAll()
    {
        foreach (var t in Pool.Where(t => t.Texture.Id > 0)) Raylib.UnloadTexture(t.Texture);

        Pool.Clear();
        _needsSort = true;
    }

    public static void RenderAll()
    {
        if (Pool.Count == 0)
            return;

        if (_needsSort)
        {
            Pool.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
            _needsSort = false;
        }

        foreach (var s in Pool.Where(s => s is { Active: true, Visible: true }))
        {
            if (s.IsDirty)
                s.UpdateTransform();

            if (s.Texture.Id > 0)
                Raylib.DrawTexturePro(
                    s.Texture,
                    s.SourceRect,
                    s.DestRect,
                    s.OriginPoint,
                    s.Rotation,
                    s.Tint
                );
            else
                Raylib.DrawRectanglePro(
                    s.DestRect,
                    s.OriginPoint,
                    s.Rotation,
                    s.EmptyColor
                );
        }
    }

    public static void RenderDebug()
    {
        if (_debugFlags == SpriteDebugFlags.None)
            return;

        foreach (var s in Pool.Where(s => s is { Active: true, Visible: true }))
        {
            if (_debugFlags.HasFlag(SpriteDebugFlags.Origin))
                Raylib.DrawCircleV(s.Position, 3, DebugColorOrigin);

            if (_debugFlags.HasFlag(SpriteDebugFlags.Box))
            {
                var rect = s.DestRect;
                rect.X -= s.OriginPoint.X;
                rect.Y -= s.OriginPoint.Y;
                Raylib.DrawRectangleLinesEx(rect, 1, DebugColorBox);
            }

            if (_debugFlags.HasFlag(SpriteDebugFlags.Name))
                Raylib.DrawText(
                    s.Name,
                    (int)s.Position.X + 10,
                    (int)s.Position.Y - 10,
                    10,
                    DebugColorOrigin
                );
        }
    }

    public static void SetDebugMode(SpriteDebugFlags flags)
    {
        _debugFlags = flags;
    }

    public static void MarkDirty()
    {
        _needsSort = true;
    }
}