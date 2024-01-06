using System.Collections.Generic;
using UnityEngine;
using VoronatorSharp;
using Random = UnityEngine.Random;

// DOC VORONATOR : https://github.com/BorisTheBrave/voronator-sharp?tab=readme-ov-file#edge-cases

public class VoronoiPolygonsGenerator : MonoBehaviour
{
    public Material mat;
    private Vector2[] _germes;
    public int regionAmount = 10;
    public Vector2Int imageDim = new Vector2Int(1920, 1080);
    private Color[] _colorsRegions;

    public Voronator voronatorDiagram;

    public List<GameObject> cells;
    public List<int> lockedCellsIds;

    void Start()
    {
        _germes = new Vector2[regionAmount];
        _colorsRegions = new Color[regionAmount];
        cells = new List<GameObject>(regionAmount);
        lockedCellsIds = new List<int>(regionAmount);

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
                    //position = new Vector3(_germes[i].x, 0, _germes[i].y)
                },
                name = "poly" + i
            };

            MeshFilter mf = go.AddComponent<MeshFilter>();

            Vector3[] vertices = listV2toV3Array(voronatorDiagram.GetClippedPolygon(i));
            mf.mesh.vertices = vertices;

            int[] trig = new int[(vertices.Length - 2) * 3];

            int trianglePoint = 1;
            for (int j = 0; j < trig.Length; j++)
            {
                if (j % 3 == 0)
                {
                    trig[j] = 0;
                }
                else
                {
                    trig[j] = trianglePoint;
                    if (j % 3 == 1)
                    {
                        trianglePoint++;
                    }
                }
            }

            mf.mesh.triangles = trig;

            go.AddComponent<MeshRenderer>().material = mat;
            cells.Add(go);
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
}