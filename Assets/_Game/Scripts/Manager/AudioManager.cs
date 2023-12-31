﻿using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioData audioData;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;
    
    private void Awake()
    {
        PlayBgm(BgmType.MainMenu);
    }

    public AudioClip GetBgm(BgmType type)
    {
        return GetAudio(audioData.BGMDic, type);
    }

    public AudioClip GetSfx(SfxType type)
    {
        return GetAudio(audioData.SfxDic, type);
    }

    private static AudioClip GetAudio<T>(IReadOnlyDictionary<T, AudioClip> audioDictionary, T type)
    {
        return audioDictionary.TryGetValue(type, out AudioClip audioClip) ? audioClip : null;
    }

    public void PlayBgm(BgmType type, float fadeFloat = 0.3f)
    {
        bgm.loop = true;
        if (fadeFloat == 0f || bgm.mute) PlayAudio(bgm, audioData.BGMDic, type);
        // FadeOut, Then Play
        else DOVirtual.Float(1, 0, fadeFloat, value => VolumeDown(bgm, value))
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                PlayAudio(bgm, audioData.BGMDic, type);
                bgm.volume = 1;
            });
    }

    public void PlaySfx(SfxType type)
    {
        PlayAudio(sfx, audioData.SfxDic, type);
    }

    private static void VolumeDown(AudioSource audioSource, float value)
    {
        audioSource.volume = value;
    }
    
    private static void PlayAudio<T>(AudioSource audioSource, IReadOnlyDictionary<T, AudioClip> audioDictionary, T type)
    {
        AudioClip audioClip = GetAudio(audioDictionary, type);
        if (audioClip == null) return;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayRandom(List<SfxType> sfxTypes)
    {
        List<AudioClip> audioClips = new();
        for (int i = 0; i < sfxTypes.Count; i++)
        {
            AudioClip audioClip = audioData.SfxDic.TryGetValue(sfxTypes[i], out AudioClip sfx1) ? sfx1 : null;
            if (audioClip == null) continue;
            audioClips.Add(audioClip);
        }

        if (audioClips.Count == 0) return;
        bgm.clip = audioClips[Random.Range(0, audioClips.Count)];
        bgm.Play();
    }

    public void PauseBgm()
    {
        bgm.Pause();
    }

    public void UnPauseBgm()
    {
        bgm.Play();
    }

    public void PauseSfx()
    {
        sfx.Pause();
    }

    public void UnPauseSfx()
    {
        sfx.Play();
    }

    public void StopSfx()
    {
        sfx.Stop();
    }
    
    public void MuteBgm()
    {
        bgm.mute = true;
    }

    public void UnMuteBgm()
    {
        bgm.mute = false;
    }

    public void MuteSfx()
    {
        sfx.mute = true;
    }

    public void UnMuteSfc()
    {
        sfx.mute = false;
    }
}
