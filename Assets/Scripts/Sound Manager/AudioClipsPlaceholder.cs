using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedAudioClipInfo
{
    public readonly AudioClip Clip;
    public readonly float Time;
    
    public PlayedAudioClipInfo(AudioClip clip, float time)
    {
        Clip = clip;
        Time = time;
    }
}

[Serializable]
public struct AudioClipPair
{
    public string clipName;
    public AudioClip clip;
}

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(AudioRoute))]
public class AudioClipsPlaceholder : MonoBehaviour
{
    [SerializeField] private List<AudioClipPair> audioClips;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play clips by giving the clip name set in the list of clip pairs
    /// </summary>
    /// <param name="clipName">The clip name as set in the list</param>
    /// <param name="loop">Whether to loop clip</param>
    /// <param name="returnToPrevLoopedClip">Whether to return to previous looped clip after finishing playing given sound</param>
    public void Play(string clipName, bool loop = false, bool returnToPrevLoopedClip = false)
    {
        if (loop)
        {
            PlayLooped(clipName);
            return;
        }
        if (returnToPrevLoopedClip)
        {
            var returnToClipInfo = new PlayedAudioClipInfo(GetClipFromList(clipName), _audioSource.time);
            StartCoroutine(ReturnToLoopedClip(returnToClipInfo));
        }
        _audioSource.loop = false;
        _audioSource.PlayOneShot(GetClipFromList(clipName));
    }
    
    public void Stop()
    {
        _audioSource.Stop();
    }

    private IEnumerator ReturnToLoopedClip(PlayedAudioClipInfo returnToClipInfo)
    {
        yield return new WaitForEndOfFrame();
        if (_audioSource.isPlaying)
        {
            StartCoroutine(ReturnToLoopedClip(returnToClipInfo));
            yield return null;
        }
        PlayLooped(returnToClipInfo.Clip, returnToClipInfo.Time);
    }
    
    private void PlayLooped(string clipName)
    {
        _audioSource.clip = GetClipFromList(clipName);
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    private void PlayLooped(AudioClip clip, float time)
    {
        _audioSource.clip = clip;
        _audioSource.time = time;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private AudioClip GetClipFromList(string clipName)
    {
        return audioClips.Find(pair => pair.clipName == clipName).clip;
    }
}
