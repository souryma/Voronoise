// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    public Vector2Int imageDim;
    public int regionAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int[] centroids = GenerateDiagram();
        }
    }

    Vector2Int[] GenerateDiagram()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        
        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
        }

        return centroids;
    }
}
