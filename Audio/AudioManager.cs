using Raylib_cs;

namespace Sunako.Audio;

public static class AudioManager
{
    private static readonly Dictionary<string, Music> Bgm = new();
    private static readonly Dictionary<string, Sound> Sfx = new();

    private static Music _currentBgm;
    private static bool _hasBgm;

    private static float BgmVolume { get; set; } = 1f;
    private static float SfxVolume { get; set; } = 1f;

    public static void Init()
    {
        if (!Raylib.IsAudioDeviceReady())
            Raylib.InitAudioDevice();
    }

    public static void LoadBgm(string key, string path)
    {
        var music = Raylib.LoadMusicStream(path);
        Bgm[key] = music;
    }

    public static void LoadSfx(string key, string path)
    {
        var sound = Raylib.LoadSound(path);
        Sfx[key] = sound;
    }

    public static void PlayBgm(string key, bool loop = true)
    {
        if (!_hasBgm)
            Raylib.StopMusicStream(_currentBgm);

        if (!Bgm.TryGetValue(key, out var music)) return;
        _currentBgm = music;
        _hasBgm = true;

        Raylib.PlayMusicStream(_currentBgm);
        _currentBgm.Looping = loop;
        Raylib.SetMusicVolume(_currentBgm, BgmVolume);
    }

    public static void StopBgm()
    {
        if (!_hasBgm) return;
        Raylib.StopMusicStream(_currentBgm);
        _hasBgm = false;
    }

    public static void CleanUp()
    {
        foreach (var music in Bgm.Values)
            Raylib.UnloadMusicStream(music);

        foreach (var sound in Sfx.Values)
            Raylib.UnloadSound(sound);

        if (Raylib.IsAudioDeviceReady())
            Raylib.CloseAudioDevice();
    }

    public static void PlaySfx(string key, float pitch = 1f, float volume = -1f)
    {
        if (!Sfx.TryGetValue(key, out var sound)) return;
        
        var vol = volume < 0 ? SfxVolume : Math.Clamp(volume, 0f, 1f);
        Raylib.SetSoundVolume(sound, vol);
        Raylib.SetSoundPitch(sound, pitch);
        Raylib.PlaySound(sound);
    }

    public static void SetBgmVolume(float volume)
    {
        BgmVolume = Math.Clamp(volume, 0f, 1f);
        if (_hasBgm)
            Raylib.SetMusicVolume(_currentBgm, BgmVolume);
    }

    public static void SetSfxVolume(float volume)
    {
        SfxVolume = Math.Clamp(volume, 0f, 1f);
    }
}