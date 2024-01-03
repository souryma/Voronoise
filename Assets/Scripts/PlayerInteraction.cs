using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public VoronoiPolygonsGenerator voronoi;
    public MusicSampleManager musicManager;

    public Transform player;

    // The cell the player is in
    private int _playerCell = 0;
    private int _previousPlayerCell = 1;

    private bool _isMusicPlaying = false;

    public void Update()
    {
        _playerCell =
            voronoi.voronatorDiagram.Find(new Vector2(player.position.x, -player.position.z), _previousPlayerCell);
        if (_playerCell != _previousPlayerCell)
        {
            if (_isMusicPlaying)
                musicManager.FadeFirstGeneratedSound();

            voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f),
                Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            musicManager.GenerateRandomSound();

            // Fade previous cell to black in 5 seconds
            StartCoroutine(FadeToBlack(5, voronoi.cells[_previousPlayerCell].GetComponent<MeshRenderer>().material));
            _previousPlayerCell = _playerCell;
            _isMusicPlaying = true;
        }
    }

    private IEnumerator FadeToBlack(float duration, Material mat)
    {
        float currentTime = 0;
        Color start = mat.color;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            mat.color = Color.Lerp(start, Color.black, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}