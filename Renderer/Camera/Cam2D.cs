using System.Numerics;
using Raylib_cs;

namespace Sunako.Renderer.Camera;

public class Cam2D : ICamera
{
    private Camera2D _camera = new()
    {
        Zoom = 1.0f,
        Offset = Vector2.Zero,
        Target = Vector2.Zero,
        Rotation = 0.0f
    };
    private float _shakeDuration;
    private float _shakeIntensity;
    private Vector2 _shakeOffset;

    public Vector2 Position
    {
        get => _camera.Target;
        set => _camera.Target = value;
    }

    public Vector2 Offset
    {
        get => _camera.Offset;
        set => _camera.Offset = value;
    }

    public float Zoom
    {
        get => _camera.Zoom;
        set => _camera.Zoom = value;
    }

    public float Rotation
    {
        get => _camera.Rotation;
        set => _camera.Rotation = value;
    }

    public void Begin()
    {
        var originalTarget = _camera.Target;
        _camera.Target = new Vector2(MathF.Round(_camera.Target.X + _shakeOffset.X), MathF.Round(_camera.Target.Y + _shakeOffset.Y));
        Raylib.BeginMode2D(_camera);
        _camera.Target = originalTarget;
    }

    public void End()
    {
        Raylib.EndMode2D();
    }

    public void Update(float dt)
    {
        if (!(_shakeDuration > 0)) return;
        _shakeDuration -= dt;
        _shakeOffset = new Vector2(
            Raylib.GetRandomValue(-100, 100) / 100.0f * _shakeIntensity,
            Raylib.GetRandomValue(-100, 100) / 100.0f * _shakeIntensity
        );

        if (!(_shakeDuration <= 0)) return;
        _shakeDuration = 0;
        _shakeOffset = Vector2.Zero;
    }

    public void Shake(float intensity, float duration)
    {
        _shakeIntensity = intensity;
        _shakeDuration = duration;
    }

    public void Follow(Vector2 targetPos, float lerpFactor, float dt)
    {
        Position = Vector2.Lerp(Position, targetPos, 1.0f - MathF.Exp(-lerpFactor * dt));
    }

    public Vector2 WorldToScreen(Vector2 worldPos)
    {
        return Raylib.GetWorldToScreen2D(worldPos, _camera);
    }

    public Vector2 ScreenToWorld(Vector2 screenPos)
    {
        return Raylib.GetScreenToWorld2D(screenPos, _camera);
    }

    public void CenterOn(Vector2 pos, int screenWidth, int screenHeight)
    {
        Position = pos;
        Offset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
    }
}