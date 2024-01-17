using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicSampleManager : MonoBehaviour
{
    private VoronoiPolygonsGenerator _voronoi;
    
    public GameObject cellPrefab;
    public List<AudioCell> audioCells;
    
    public List<AudioClip> piano;
    public List<AudioClip> drum;
    public List<AudioClip> choir;
    public List<AudioClip> guitar;
    public List<AudioClip> soundBank5;
    public List<AudioClip> soundBank6;
    
    public List<AudioClip> soundBank;

    private void Start()
    {
        _voronoi = GameObject.Find("Voronoi").GetComponent<VoronoiPolygonsGenerator>();

        int numberOfSoundBanks = Random.Range(1, 3);

        for (int i = 0; i < numberOfSoundBanks; i++)
        {
            // Select random sound bank
            switch (Random.Range(1, 7))
            {
                case 1: soundBank.AddRange(piano);
                    break;
                case 2: soundBank.AddRange(drum);
                    break;
                case 3: soundBank.AddRange(choir);
                    break;
                case 4: soundBank.AddRange(guitar);
                    break;
                case 5: soundBank.AddRange(soundBank5);
                    break;
                case 6: soundBank.AddRange(soundBank6);
                    break;
            }
        }
    }

    public void GenerateRandomSound(int cellId, MeshRenderer cell)
    {
        GameObject newObject = Instantiate(cellPrefab, transform, true) as GameObject;
        newObject.name = "MusicSample" + cellId;
        AudioCell newAudioCell = newObject.AddComponent<AudioCell>();

        // Sound creation
        newAudioCell.Create(cell, cellId, soundBank[Random.Range(0, soundBank.Count)], Random.Range(0.2f, 0.8f),
            Random.Range(0.4f, 2),
            1);

        audioCells.Add(newAudioCell);
    }

    // Fade then destroy the music cell after checking if it wasn't locked
    public void FadeOutMusicCell(int id)
    {
        AudioCell cellToDestroy = audioCells.Find(cell => cell.id == id);

        // Check if the cell is not locked
        if (_voronoi.lockedCellsIds.Contains(cellToDestroy.id))
            return;

        StartCoroutine(cellToDestroy.FadeAndDestroy());
        audioCells.Remove(cellToDestroy);
    }

    public void FadeFirstGeneratedSound()
    {
        AudioCell cellToDestroy = audioCells[0];

        // Check if the cell is not locked
        if (!_voronoi.lockedCellsIds.Contains(cellToDestroy.id))
            return;

        StartCoroutine(cellToDestroy.FadeAndDestroy());
        audioCells.Remove(cellToDestroy);
    }
}