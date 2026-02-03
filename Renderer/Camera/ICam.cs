using System.Numerics;

namespace Sunako.Renderer.Camera;

public interface ICamera
{
    void Begin();
    void End();
    Vector2 WorldToScreen(Vector2 worldPos);
    Vector2 ScreenToWorld(Vector2 screenPos);
}