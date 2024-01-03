using System.Collections.Generic;
using UnityEngine;

public class MusicSampleManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public List<AudioCell> audioCells;
    public List<AudioClip> soundBank;

    public void GenerateRandomSound()
    {
        GameObject newObject = Instantiate(cellPrefab, transform, true) as GameObject;
        AudioCell newAudioCell = newObject.AddComponent<AudioCell>();
        
        // Sound creation
        newAudioCell.Create(soundBank[Random.Range(0, soundBank.Count)], Random.Range(0.5f, 1), Random.Range(0.4f, 2),
            1);

        audioCells.Add(newAudioCell);
    }

    public void FadeFirstGeneratedSound()
    {
        AudioCell cellToDestroy = audioCells[0];
        StartCoroutine(cellToDestroy.FadeAndDestroy());
        audioCells.Remove(cellToDestroy);
    }
}