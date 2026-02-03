using Raylib_cs;

namespace Sunako.Renderer.TileMap;

public class Tileset
{
    public Texture2D Texture { get; private set; }
    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }
    public int Spacing { get; private set; }
    public int Margin { get; private set; }

    public int Columns { get; private set; }
    public int Rows { get; private set; }

    public Tileset(string texturePath, int tileWidth, int tileHeight, int spacing = 0, int margin = 0)
    {
        Texture = Raylib.LoadTexture(texturePath);
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Spacing = spacing;
        Margin = margin;

        CalculateDimensions();
    }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight, int spacing = 0, int margin = 0)
    {
        Texture = texture;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Spacing = spacing;
        Margin = margin;

        CalculateDimensions();
    }

    private void CalculateDimensions()
    {
        if (Texture.Id == 0) return;

        Columns = (Texture.Width - Margin * 2 + Spacing) / (TileWidth + Spacing);
        Rows = (Texture.Height - Margin * 2 + Spacing) / (TileHeight + Spacing);
    }

    public Rectangle GetSourceRect(int id)
    {
        if (id < 0 || Columns == 0) return new Rectangle(0, 0, 0, 0);

        var col = id % Columns;
        var row = id / Columns;

        float x = Margin + col * (TileWidth + Spacing);
        float y = Margin + row * (TileHeight + Spacing);

        return new Rectangle(x, y, TileWidth, TileHeight);
    }

    private readonly Dictionary<int, Physics.Aabb> _tileColliders = new();

    public void SetTileCollision(int id, Physics.Aabb box)
    {
        _tileColliders[id] = box;
    }

    public Physics.Aabb? GetTileCollision(int id)
    {
        if (_tileColliders.TryGetValue(id, out var box))
            return box;
        return null;
    }

    public void Unload()
    {
        if (Texture.Id != 0) Raylib.UnloadTexture(Texture);
    }
}
