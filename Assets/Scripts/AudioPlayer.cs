using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> soundBank;

    [Range(0, 1)]
    public int soundToPlay = 1;

    // Pitch of the sound (tempo + height)
    [Range(-1, 5)]
    public float pitch = 1;

    // The height of the sound (0 = low, 2 = high, 1 = normal)
    [Range(0, 2)]
    public float noteHeight = 1;

    private AudioMixer _audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        _audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = pitch;
        _audioMixer.SetFloat("pitchBend", noteHeight);

        if (audioSource.isPlaying)
        {
            return;
        }
        
        audioSource.clip = soundBank[soundToPlay];
        audioSource.Play();
    }
}