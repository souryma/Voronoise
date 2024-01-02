using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VoronatorSharp;

// DOC VORONATOR : https://github.com/BorisTheBrave/voronator-sharp?tab=readme-ov-file#edge-cases

public class TestVoroSharp : MonoBehaviour
{
    public Material mat;
    private Vector2[] _germes;
    public int regionAmount = 10;
    public Vector2Int imageDim = new Vector2Int(1920, 1080);
    private Color[] _colorsRegions;
    private Color[] _pixelColors;

    private Voronator _voronatorDiagram;

    void Start()
    {
        _germes = new Vector2[regionAmount];
        _colorsRegions = new Color[regionAmount];
        _pixelColors = new Color[imageDim.x * imageDim.y];

        // Generates random cells position
        for (int i = 0; i < regionAmount; i++)
        {
            _germes[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
            _colorsRegions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }

        // Generate voronator diagram
        _voronatorDiagram = new Voronator(_germes, new Vector2(0, 0), new Vector2(imageDim.x, imageDim.x));
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < regionAmount; i++)
            {
                GameObject go = new GameObject();
                go.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0, 0));

                // TODO : make the scale good
                go.transform.position = new Vector3(-4.7f, 0, 4.4f);
                go.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
                
                go.name = "poly" + i;
                MeshFilter mf = go.AddComponent<MeshFilter>();

                var vertices = listV2toV3Array(_voronatorDiagram.GetClippedPolygon(i));
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
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            // Generate image by coloring each pixels by the color of the polygons the pixel is in
            for (int x = 0; x < imageDim.x; x++)
            {
                for (int y = 0; y < imageDim.y; y++)
                {
                    int index = x * imageDim.x + y;
                    _pixelColors[index] = _colorsRegions[_voronatorDiagram.Find(new Vector2(x, y))];
                }
            }

            GetComponent<SpriteRenderer>().sprite = Sprite.Create(GetImageFromColorArray(_pixelColors),
                new Rect(0, 0, imageDim.x, imageDim.y),
                Vector2.one * 0.5f);
        }
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

    Texture2D GetImageFromColorArray(Color[] pc)
    {
        Texture2D tex = new Texture2D(imageDim.x, imageDim.y);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(pc);
        tex.Apply();
        return tex;
    }
}