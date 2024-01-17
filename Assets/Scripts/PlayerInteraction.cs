using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerInteraction : MonoBehaviour
{
    private VoronoiPolygonsGenerator _voronoi;
    public MusicSampleManager musicManager;

    public Transform player;

    // The cell the player is in
    private int _playerCell = 0;
    private int _previousPlayerCell = 1;

    private bool _isMusicPlaying = false;

    public KeyCode lockCellKey = KeyCode.Space;

    private Vector2Int[] _colorSets =
    {
        // Min and max hues, in degrees
        new Vector2Int(220, 310),
        new Vector2Int(140, 200),
        new Vector2Int(0, 60),
        new Vector2Int(20, 40)
    };

    public Sprite happy;
    public Sprite smile;
    public Sprite weird;
    public Sprite noMouth;
    public Sprite lost;
    public Sprite melt;
    public Sprite POOP;

    public SpriteRenderer playerSprite;

    private int _colorSet;

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerSprite.sprite = happy;
        _voronoi = GameObject.Find("Voronoi").GetComponent<VoronoiPolygonsGenerator>();

        if (musicManager.isCACA)
            _colorSet = _colorSets.Length - 1;
        else 
            _colorSet = Random.Range(0, _colorSets.Length);
    }

    private void RebootScene()
    {
        // Danger
        _voronoi.Reboot();
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape))
        {
            RebootScene();
        }

        // Get the id of the cell where the player is
        _playerCell =
            _voronoi.voronatorDiagram.Find(new Vector2(player.position.x, -player.position.z), _previousPlayerCell);

        // Check if its a different cell than before
        if (_playerCell != _previousPlayerCell)
        {
            if (_isMusicPlaying)
                musicManager.FadeOutMusicCell(_previousPlayerCell);

            // Check if the cell is not already locked
            if (!_voronoi.lockedCellsIds.Contains(_playerCell))
            {
                _voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(
                    Random.Range(
                        _colorSets[_colorSet].x / 360f,
                        _colorSets[_colorSet].y / 360f),
                    0.5f,
                    1f);

                musicManager.GenerateRandomSound(_playerCell, _voronoi.cells[_playerCell].GetComponent<MeshRenderer>());
            }

            // If the last cell to destroy is not locked
            if (!_voronoi.lockedCellsIds.Contains(_previousPlayerCell))
            {
                // Fade previous cell to black in 5 seconds
                // StartCoroutine(FadeToBlack(5, _voronoi.cells[_previousPlayerCell].GetComponent<MeshRenderer>().material));
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
        if (_voronoi.lockedCellsIds.Contains(_playerCell))
        {
            // If the cell is already locked, we unlock it
            _voronoi.lockedCellsIds.Remove(_playerCell);
            musicManager.audioCells.Find(cell => cell.id == _playerCell).LockCell(false);
            //_voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 1, 1f);
        }
        else
        {
            _voronoi.lockedCellsIds.Add(_playerCell);
            musicManager.audioCells.Find(cell => cell.id == _playerCell).LockCell(true);
            //_voronoi.cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1f);
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