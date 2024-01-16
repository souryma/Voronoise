using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicSampleManager : MonoBehaviour
{
    private VoronoiPolygonsGenerator _voronoi;
    
    public GameObject cellPrefab;
    public List<AudioCell> audioCells;
    public List<AudioClip> soundBank;

    private void Start()
    {
        _voronoi = GameObject.Find("Voronoi").GetComponent<VoronoiPolygonsGenerator>();
    }

    public void GenerateRandomSound(int cellId)
    {
        GameObject newObject = Instantiate(cellPrefab, transform, true) as GameObject;
        newObject.name = "MusicSample" + cellId;
        AudioCell newAudioCell = newObject.AddComponent<AudioCell>();

        // Sound creation
        newAudioCell.Create(cellId, soundBank[Random.Range(0, soundBank.Count)], Random.Range(0.2f, 0.8f),
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