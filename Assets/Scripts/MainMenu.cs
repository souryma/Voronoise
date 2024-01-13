using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public VoronoiPolygonsGenerator voronoi;
    public void PlayGame()
    {
        voronoi.StartGame();
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void QuitGame()
    {
        Debug.LogWarning("Cannot quit in editor mode");
        Application.Quit();
    }
}
