using Raylib_cs;

namespace Sunako.Renderer.AnimatedSprite;

public class SpriteFrames
{
    private readonly Dictionary<string, List<Rectangle>> _anims = new();

    public void Add(string name, Rectangle frame)
    {
        if (!_anims.ContainsKey(name))
            _anims[name] = [];

        _anims[name].Add(frame);
    }

    public List<Rectangle>? Get(string name)
    {
        return _anims.GetValueOrDefault(name);
    }
}