using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public VoronoiPolygonsGenerator voronoi;
    public MusicSampleManager musicManager;

    public Transform player;

    // The cell the player is in
    private int _playerCell = 0;
    private int _previousPlayerCell = 1;

    private bool _isMusicPlaying = false;

    public KeyCode lockCellKey = KeyCode.Space;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
        // Get the id of the cell where the player is
        _playerCell =
            voronoi.voronatorDiagram.Find(new Vector2(player.position.x, -player.position.z), _previousPlayerCell);

        // Check if its a different cell than before
        if (_playerCell != _previousPlayerCell)
        {
            if (_isMusicPlaying)
                musicManager.FadeOutMusicCell(_previousPlayerCell);

            // Check if the cell is not already locked
            if (!voronoi.lockedCellsIds.Contains(_playerCell))
            {
                voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f),
                    Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
                musicManager.GenerateRandomSound(_playerCell);
            }

            // If the last cell to destroy is not locked
            if (!voronoi.lockedCellsIds.Contains(_previousPlayerCell))
            {
                // Fade previous cell to black in 5 seconds
                StartCoroutine(FadeToBlack(5,
                    voronoi.cells[_previousPlayerCell].GetComponent<MeshRenderer>().material));
            }

            _previousPlayerCell = _playerCell;
            _isMusicPlaying = true;
        }

        if (Input.GetKeyDown(lockCellKey) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            LockCell();
        }
    }

    private void LockCell()
    {
        if (voronoi.lockedCellsIds.Contains(_playerCell))
        {
            // If the cell is already locked, we unlock it
            voronoi.lockedCellsIds.Remove(_playerCell);
            voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 1, 1f);
        }
        else
        {
            voronoi.lockedCellsIds.Add(_playerCell);
            voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1f);
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