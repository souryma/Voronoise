using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioCell : MonoBehaviour
{
    private Material _cellMaterial;
    private Color _cellActualColor;
    private Color _cellBaseColor;
    private Color _cellFadeColor;

    public AudioCell Create(MeshRenderer cell, int _id, AudioClip _clip, float _volume, float _pitch, float _noteHeight)
    {
        _cellMaterial = cell.material;
        _cellActualColor = _cellMaterial.color;
        _cellBaseColor = _cellMaterial.color;
        _cellFadeColor = new Color(_cellActualColor.r / 2f, _cellActualColor.g / 2f, _cellActualColor.b / 2f);
        id = _id;
        clip = _clip;
        volume = _volume;
        pitch = _pitch;
        noteHeight = _noteHeight;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        _audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
        UpdateSoundParameters();
        //FadeIn(2.0f);

        return this;
    }

    public AudioSource audioSource;
    public AudioClip clip;
    public int id;

    [Range(0, 1)] public float volume = 1;

    // Pitch of the sound (tempo + height)
    [Range(0, 5)] public float pitch = 1;

    // The height of the sound (0 = low, 2 = high, 1 = normal)
    [Range(0, 2)] public float noteHeight = 1;

    private AudioMixer _audioMixer;
    private float _fadeOutDuration = 5.0f;

    private Coroutine _colorCoroutine;

    private bool _isFirstTurn = true;

    // Update is called once per frame
    private void Update()
    {
        UpdateSoundParameters();

        // Manual loop
        if (audioSource.isPlaying)
        {
            return;
        }

        if (!_isFirstTurn)
            StopCoroutine(_colorCoroutine);
        _isFirstTurn = true;

        audioSource.Play();

        _cellMaterial.color = _cellActualColor;
        _colorCoroutine = StartCoroutine(FadeColor(audioSource.clip.length, _cellMaterial, _cellFadeColor, false));
    }

    // Color the cell in white when its locked
    public void LockCell(bool state)
    {
        _cellActualColor = state ? Color.white : _cellBaseColor;
    }

    private void UpdateSoundParameters()
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        _audioMixer.SetFloat("pitchBend", noteHeight);
    }

    public IEnumerator FadeAndDestroy()
    {
        //StopCoroutine(_colorCoroutine);

        FadeOutMusic(_fadeOutDuration);
        StartCoroutine(FadeColor(_fadeOutDuration, _cellMaterial, Color.black, true));
        yield return new WaitForSeconds(_fadeOutDuration);

        _cellMaterial.color = Color.black;
        Destroy(gameObject);
    }

    private void FadeIn(float duration)
    {
        StartCoroutine(StartFade(duration, 50f));
    }

    private void FadeOutMusic(float duration)
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

    private IEnumerator FadeColor(float duration, Material mat, Color targetColor, bool isOverAnotherFade)
    {
        float currentTime = 0;
        Color start = mat.color;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            if (isOverAnotherFade)
                mat.color *= Color.Lerp(start, targetColor, currentTime / duration);
            else
                mat.color = Color.Lerp(start, targetColor, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}