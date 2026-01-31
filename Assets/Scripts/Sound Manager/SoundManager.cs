using UnityEngine;
using UnityEngine.Audio;

public sealed class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Exposed parameter names")]
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string sfxParam = "SFXVolume";
    
    [Header("Default Volumes")]
    [Range(0f, 1f)][SerializeField] private float defaultMusicVolume = 0.8f;
    [Range(0f, 1f)][SerializeField] private float defaultSfxVolume = 0.9f;

    private AudioSource _musicSource;

    private const string MusicKey = "vol_music";
    private const string SfxKey = "vol_sfx";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("More than one SoundManager in scene!");
            return;
        }
        Instance = this;

        var music = PlayerPrefs.GetFloat(MusicKey, defaultMusicVolume);
        var sfx = PlayerPrefs.GetFloat(SfxKey, defaultSfxVolume);

        SetMusicVolume(music, save: false);
        SetSfxVolume(sfx, save: false);
    }

    public float GetMusicVolume01() => PlayerPrefs.GetFloat(MusicKey, defaultMusicVolume);
    public float GetSfxVolume01() => PlayerPrefs.GetFloat(SfxKey, defaultSfxVolume);

    public void SetMusicVolume(float v01, bool save = true)
    {
        v01 = Mathf.Clamp01(v01);
        mixer.SetFloat(musicParam, LinearToDb(v01));
        if (save) PlayerPrefs.SetFloat(MusicKey, v01);
    }

    public void SetSfxVolume(float v01, bool save = true)
    {
        v01 = Mathf.Clamp01(v01);
        mixer.SetFloat(sfxParam, LinearToDb(v01));
        if (save) PlayerPrefs.SetFloat(SfxKey, v01);
    }
    
    /// <summary>
    /// Play music function for playing audio clips on music channel
    /// on background and (no world space relation)
    /// </summary>
    /// <param name="clip">Music audio clip</param>
    /// <param name="loop">Music audio looping</param>
    /// <param name="fadeSeconds">Fade time in/out</param>
    public void PlayMusic(AudioClip clip, bool loop = true, float fadeSeconds = 0f)
    {
        if (_musicSource == null || clip == null) return;

        _musicSource.loop = loop;
        if (fadeSeconds <= 0f)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
            return;
        }

        // "swap with fade out/in" helper function
        StartCoroutine(FadeSwapMusic(clip, fadeSeconds));
    }

    public void StopMusic(float fadeSeconds = 0f)
    {
        if (_musicSource == null) return;
        if (fadeSeconds <= 0f) { _musicSource.Stop(); return; }
        StartCoroutine(FadeOutMusic(fadeSeconds));
    }

    // --- Utility ---
    private static float LinearToDb(float v01)
    {
        // v01 = 0 should be effectively silent. -80dB is Unity-ish "silence".
        if (v01 <= 0.0001f) return -80f;
        return Mathf.Log10(v01) * 20f;
    }

    /// <summary>
    /// A rudimentary simple in/out music fade
    /// using volume channels to fade
    /// </summary>
    /// <param name="newClip">New music audio clip to fade into</param>
    /// <param name="seconds">Fade time in seconds</param>
    /// <returns></returns>
    private System.Collections.IEnumerator FadeSwapMusic(AudioClip newClip, float seconds)
    {
        var startVol = _musicSource.volume;
        for (float t = 0; t < seconds; t += Time.unscaledDeltaTime)
        {
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / seconds);
            yield return null;
        }
        _musicSource.volume = 0f;

        _musicSource.clip = newClip;
        _musicSource.Play();

        for (float t = 0; t < seconds; t += Time.unscaledDeltaTime)
        {
            _musicSource.volume = Mathf.Lerp(0f, startVol, t / seconds);
            yield return null;
        }
        _musicSource.volume = startVol;
    }

    private System.Collections.IEnumerator FadeOutMusic(float seconds)
    {
        var startVol = _musicSource.volume;
        for (float t = 0; t < seconds; t += Time.unscaledDeltaTime)
        {
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / seconds);
            yield return null;
        }
        _musicSource.Stop();
        _musicSource.volume = startVol;
    }
}
