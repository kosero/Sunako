using Raylib_cs;
using Sunako.Audio;
using Sunako.Renderer.Sprite;

namespace Sunako.Core;

public class Window()
{
    private static bool _isRunning;
    private static bool _fullscreen;
    private static bool _vsync;
    private static bool _windowIsResizable;
    private static bool _msaa;

    private string _title;
    private int _width;
    private int _height;

    protected Window(string title, int width, int height) : this()
    {
        _title = title;
        _width = width;
        _height = height;
        _isRunning = true;
    }

    public Window WithFullscreen(bool value = true)
    {
        _fullscreen = value;
        return this;
    }

    public Window WithVsync(bool value = true)
    {
        _vsync = value;
        return this;
    }

    public Window WithResizable(bool value = true)
    {
        _windowIsResizable = value;
        return this;
    }

    public Window WithMsaa(bool value = true)
    {
        _msaa = value;
        return this;
    }

    public Window WithIcon(string path = "icon.png")
    {
        Raylib.SetWindowIcon(Raylib.LoadImage(path));
        return this;
    }

    public static void Close()
    {
        _isRunning = false;
    }

    protected virtual void LoadContent()
    {
    }

    protected virtual void Update(float delta)
    {
    }

    protected virtual void Render()
    {
    }

    protected virtual void CleanUp()
    {
    }

    public void Run()
    {
        ConfigFlags flags = 0;
        if (_vsync) flags |= ConfigFlags.VSyncHint;
        if (_msaa) flags |= ConfigFlags.Msaa4xHint;
        if (_windowIsResizable) flags |= ConfigFlags.ResizableWindow;
        if (_fullscreen) flags |= ConfigFlags.FullscreenMode;

        Raylib.SetConfigFlags(flags);
        Raylib.InitWindow(_width, _height, _title);
        AudioManager.Init();
        Raylib.SetExitKey(KeyboardKey.Null);

        SpriteManager.InitPool();
        LoadContent();
        
        while (_isRunning && !Raylib.WindowShouldClose())
        {
            var delta = Raylib.GetFrameTime();
            Update(delta);
            Raylib.BeginDrawing();
            Render();
            Raylib.EndDrawing();
        }

        CleanUp();
        SpriteManager.DestroyAll();
        AudioManager.CleanUp();
        Raylib.CloseWindow();
    }
}