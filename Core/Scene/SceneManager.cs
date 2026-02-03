namespace Sunako.Core.Scene;

public static class SceneManager
{
    private static readonly Stack<IScene> Stack = new();

    private static IScene? Current =>
        Stack.Count > 0 ? Stack.Peek() : null;

    public static void Push(IScene scene)
    {
        if (Stack.Count > 0)
            Stack.Peek().OnPause();

        Stack.Push(scene);
        scene.OnEnter();
    }

    public static void Pop()
    {
        if (Stack.Count == 0)
            return;

        var top = Stack.Pop();
        top.OnExit();

        if (Stack.Count > 0)
            Stack.Peek().OnResume();
    }

    public static void Replace(IScene scene)
    {
        Pop();
        Push(scene);
    }

    public static void Update(float dt)
    {
        Current?.Update(dt);
    }

    public static void Render()
    {
        Current?.Render();
    }
}
