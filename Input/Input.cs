using Raylib_cs;

namespace Sunako.Input;

public class Input
{
    private static readonly Dictionary<string, List<KeyboardKey>> Actions = new();

    public static void Bind(string action, KeyboardKey key)
    {
        if (!Actions.ContainsKey(action))
            Actions[action] = [];

        if (!Actions[action].Contains(key))
            Actions[action].Add(key);
    }

    public static bool IsActionDown(string action)
    {
        return Actions.TryGetValue(action, out var keys) && keys.Any(k => Raylib.IsKeyDown(k));
    }

    public static bool IsActionPressed(string action)
    {
        return Actions.TryGetValue(action, out var keys) && keys.Any(k => Raylib.IsKeyPressed(k));
    }

    public static float GetAxis(string negative, string positive)
    {
        float value = 0;
        if (IsActionDown(negative)) value -= 1;
        if (IsActionDown(positive)) value += 1;
        return value;
    }
}