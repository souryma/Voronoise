using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> soundBank;

    [Range(0, 10)] public int soundToPlay = 1;

    [Range(0, 1)] public float volume = 1;

    // Pitch of the sound (tempo + height)
    [Range(0, 5)] public float pitch = 1;

    // The height of the sound (0 = low, 2 = high, 1 = normal)
    [Range(0, 2)] public float noteHeight = 1;

    private AudioMixer _audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        _audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        _audioMixer.SetFloat("pitchBend", noteHeight);

        if (Input.GetKeyDown(KeyCode.G))
        {
            FadeOut3s();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            FadeIn3s();
        }

        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = soundBank[soundToPlay];
        audioSource.Play();
    }

    public void FadeIn3s()
    {
        //StartCoroutine(FadeMixerGroup.StartFade(_audioMixer, "Volume", 3.0f, 60f));
        StartCoroutine(FadeMixerGroup.StartFade(audioSource, 2.0f, 60f));
    }

    public void FadeOut3s()
    {
        //StartCoroutine(FadeMixerGroup.StartFade(_audioMixer, "Volume", 3.0f, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(audioSource, 10.0f, 0f));
    }
}