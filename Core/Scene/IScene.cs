namespace Sunako.Core.Scene;

public interface IScene
{
    void OnEnter();
    void OnExit();
    void OnPause();
    void OnResume(); 
    void Update(float dt);
    void Render();
}