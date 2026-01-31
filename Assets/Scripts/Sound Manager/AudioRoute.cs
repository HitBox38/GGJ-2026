using System;
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AudioRoute : MonoBehaviour
{
    private enum Channel { SFX, Music }

    [SerializeField] private Channel channel = Channel.SFX;

    [Header("Mixer Routing")]
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup musicGroup;

    private AudioSource _src;

    private void Reset()
    {
        _src = GetComponent<AudioSource>();
        _src.playOnAwake = false;
        _src.spatialBlend = 0f; // 2D by default for 2D games
    }

    private void Awake()
    {
        _src = GetComponent<AudioSource>();
        Apply();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            _src = GetComponent<AudioSource>();
            Apply();
        }
    }
#endif

    public void Apply()
    {
        if (_src == null) return;

        switch (channel)
        {
            case Channel.SFX:
                if (sfxGroup != null) _src.outputAudioMixerGroup = sfxGroup;
                break;
            case Channel.Music:
                if (musicGroup != null) _src.outputAudioMixerGroup = musicGroup;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}