using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioCell : MonoBehaviour
{
    public AudioCell Create(AudioClip _clip, float _volume, float _pitch, float _noteHeight)
    {
        clip = _clip;
        volume = _volume;
        pitch = _pitch;
        noteHeight = _noteHeight;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        _audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
        UpdateSoundParameters();
        FadeIn(2.0f);

        return this;
    }

    public AudioSource audioSource;
    public AudioClip clip;

    [Range(0, 1)] public float volume = 1;

    // Pitch of the sound (tempo + height)
    [Range(0, 5)] public float pitch = 1;

    // The height of the sound (0 = low, 2 = high, 1 = normal)
    [Range(0, 2)] public float noteHeight = 1;

    private AudioMixer _audioMixer;
    private float _fadeOutDuration = 10.0f;

    // Update is called once per frame
    void Update()
    {
        UpdateSoundParameters();

        // Manual loop
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.Play();
    }

    private void UpdateSoundParameters()
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        _audioMixer.SetFloat("pitchBend", noteHeight);
    }

    public IEnumerator FadeAndDestroy()
    {
        FadeOut(_fadeOutDuration);
        yield return new WaitForSeconds(_fadeOutDuration);
        Destroy(gameObject);
    }

    private void FadeIn(float duration)
    {
        StartCoroutine(StartFade(duration, 60f));
    }

    private void FadeOut(float duration)
    {
        StartCoroutine(StartFade(duration, 0f));
    }

    private IEnumerator StartFade(float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}