using System.Numerics;
using Raylib_cs;

namespace Sunako.Renderer.TileMap;

public class TilemapLayer
{
    public int[,] Grid { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool Visible { get; set; } = true;
    public float Opacity { get; set; } = 1.0f;
    public string Name { get; set; }

    private readonly Tileset _tileset;

    public TilemapLayer(Tileset tileset, int width, int height, string name = "Layer")
    {
        _tileset = tileset;
        Width = width;
        Height = height;
        Name = name;
        Grid = new int[width, height];

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            Grid[x, y] = -1;
    }

    public void SetTile(int x, int y, int id)
    {
        if (IsValid(x, y)) Grid[x, y] = id;
    }

    public int GetTile(int x, int y)
    {
        if (IsValid(x, y)) return Grid[x, y];
        return -1;
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public void Render(Vector2 position, float scale = 1.0f)
    {
        if (!Visible || Opacity <= 0f) return;

        var tint = Raylib.ColorAlpha(Color.White, Opacity);

        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var tileId = Grid[x, y];
            if (tileId < 0) continue;
            var src = _tileset.GetSourceRect(tileId);

            var destX = MathF.Round(position.X + x * _tileset.TileWidth * scale);
            var destY = MathF.Round(position.Y + y * _tileset.TileHeight * scale);
            var destW = _tileset.TileWidth * scale;
            var destH = _tileset.TileHeight * scale;

            var dest = new Rectangle(destX, destY, destW, destH);

            Raylib.DrawTexturePro(_tileset.Texture, src, dest, Vector2.Zero, 0f, tint);
        }
    }

    public List<Physics.Aabb> GetColliders(Physics.Aabb area, Vector2 mapPosition, float mapScale)
    {
        var colliders = new List<Physics.Aabb>();

        var localMin = (area.Position - mapPosition) / mapScale;
        var localMax = (area.Position + area.Size - mapPosition) / mapScale;

        var startX = (int)Math.Floor(localMin.X / _tileset.TileWidth);
        var startY = (int)Math.Floor(localMin.Y / _tileset.TileHeight);
        var endX = (int)Math.Ceiling(localMax.X / _tileset.TileWidth);
        var endY = (int)Math.Ceiling(localMax.Y / _tileset.TileHeight);

        startX = Math.Max(0, startX);
        startY = Math.Max(0, startY);
        endX = Math.Min(Width, endX);
        endY = Math.Min(Height, endY);

        for (var x = startX; x < endX; x++)
        {
            for (var y = startY; y < endY; y++)
            {
                var tileId = GetTile(x, y);
                if (tileId == -1) continue;

                var tileBox = _tileset.GetTileCollision(tileId);
                if (!tileBox.HasValue) continue;
                var box = tileBox.Value;

                var worldX = mapPosition.X + (x * _tileset.TileWidth + box.X) * mapScale;
                var worldY = mapPosition.Y + (y * _tileset.TileHeight + box.Y) * mapScale;
                var worldW = box.Width * mapScale;
                var worldH = box.Height * mapScale;

                colliders.Add(new Physics.Aabb(worldX, worldY, worldW, worldH));
            }
        }

        return colliders;
    }
}
