using System.Numerics;

namespace Sunako.Renderer.TileMap;

public class Tilemap
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Scale { get; set; } = 1.0f;
    public bool Visible { get; set; } = true;

    public Tileset Tileset { get; private set; }
    private readonly List<TilemapLayer> _layers = new();

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Tilemap(Tileset tileset, int width, int height)
    {
        Tileset = tileset;
        Width = width;
        Height = height;

        AddLayer("Base");
    }

    public TilemapLayer AddLayer(string name)
    {
        var layer = new TilemapLayer(Tileset, Width, Height, name);
        _layers.Add(layer);
        return layer;
    }

    public TilemapLayer? GetLayer(string name)
    {
        return _layers.FirstOrDefault(l => l.Name == name);
    }

    public TilemapLayer? GetLayer(int index)
    {
        if (index >= 0 && index < _layers.Count)
            return _layers[index];
        return null;
    }

    public void SetTile(int x, int y, int id, string layerName = "Base")
    {
        var layer = GetLayer(layerName);
        layer?.SetTile(x, y, id);
    }

    public void SetTile(int x, int y, int id, int layerIndex)
    {
        var layer = GetLayer(layerIndex);
        layer?.SetTile(x, y, id);
    }

    public int GetTile(int x, int y, string layerName = "Base")
    {
        var layer = GetLayer(layerName);
        return layer?.GetTile(x, y) ?? -1;
    }

    public void Render()
    {
        if (!Visible) return;

        foreach (var layer in _layers) layer.Render(Position, Scale);
    }

    public bool HasTileAtWorldPosition(Vector2 worldPos, string layerName = "Base")
    {
        var localPos = worldPos - Position;

        var tx = (int)(localPos.X / (Tileset.TileWidth * Scale));
        var ty = (int)(localPos.Y / (Tileset.TileHeight * Scale));

        return GetTile(tx, ty, layerName) != -1;
    }

    public List<Physics.Aabb> GetColliders(Physics.Aabb area, string layerName)
    {
        var layer = GetLayer(layerName);
        return layer == null ? new List<Physics.Aabb>() : layer.GetColliders(area, Position, Scale);
    }
    
    public List<Physics.Aabb> GetColliders(Physics.Aabb area)
    {
        var colliders = new List<Physics.Aabb>();
        foreach(var layer in _layers)
        {
            colliders.AddRange(layer.GetColliders(area, Position, Scale));
        }
        return colliders;
    }
}
