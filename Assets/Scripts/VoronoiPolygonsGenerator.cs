using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoronatorSharp;
using Random = UnityEngine.Random;

public class VoronoiPolygonsGenerator : MonoBehaviour
{
    public Material mat;
    private Vector2[] _germes;
    public int regionAmount = 10;
    public Vector2Int imageDim = new Vector2Int(1920, 1080);
    private Color[] _colorsRegions;

    public Voronator voronatorDiagram;

    public Transform player;

    // The cell the player is in
    private int _playerCell = 0;
    private int _previousPlayerCell = 1;

    private List<GameObject> _cells;

    void Start()
    {
        _germes = new Vector2[regionAmount];
        _colorsRegions = new Color[regionAmount];
        _cells = new List<GameObject>(regionAmount);

        // Generates random cells position
        for (int i = 0; i < regionAmount; i++)
        {
            _germes[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
            _colorsRegions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }

        // Generate voronator diagram
        voronatorDiagram = new Voronator(_germes, new Vector2(0, 0), new Vector2(imageDim.x, imageDim.x));

        // Generate polygons
        for (int i = 0; i < regionAmount; i++)
        {
            GameObject go = new GameObject
            {
                transform =
                {
                    parent = transform,
                    rotation = Quaternion.Euler(new Vector3(-90f, 0, 0)),

                    // TODO : make the scale and pos good
                    //position = new Vector3(-4.7f, 0, 4.4f),
                    //localScale = new Vector3(0.007f, 0.007f, 0.007f)
                },
                name = "poly" + i
            };

            MeshFilter mf = go.AddComponent<MeshFilter>();

            Vector3[] vertices = listV2toV3Array(voronatorDiagram.GetClippedPolygon(i));
            mf.mesh.vertices = vertices;

            int[] trig = null;

            // TODO : make it mathematic 
            switch (vertices.Length)
            {
                case 3:
                    trig = new int[] {0, 1, 2};
                    break;
                case 4:
                    trig = new int[] {0, 1, 2, 0, 2, 3};
                    break;
                case 5:
                    trig = new int[] {0, 1, 2, 0, 2, 3, 0, 3, 4};
                    break;
                case 6:
                    trig = new int[] {0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5};
                    break;
                case 7:
                    trig = new int[] {0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6};
                    break;
                case 8:
                    trig = new int[] {0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7};
                    break;
            }

            mf.mesh.triangles = trig;

            go.AddComponent<MeshRenderer>().material = mat;
            _cells.Add(go);
        }
    }

    public void Update()
    {
        _playerCell = voronatorDiagram.Find(new Vector2(player.position.x, -player.position.z), _previousPlayerCell);
        if (_playerCell != _previousPlayerCell)
        {
            _cells[_playerCell].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f),
                Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            
            // Fade previous cell to black in 5 seconds
            StartCoroutine(FadeToBlack(5, _cells[_previousPlayerCell].GetComponent<MeshRenderer>().material ));
            _previousPlayerCell = _playerCell;
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

    private Vector3[] listV2toV3Array(List<Vector2> v2)
    {
        Vector3[] v3 = new Vector3[v2.Count];
        for (int i = 0; i < v2.Count; i++)
        {
            v3[i] = v2[i];
        }

        return v3;
    }
}