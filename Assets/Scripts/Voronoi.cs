using UnityEngine;

public class Voronoi : MonoBehaviour
{
    // public Vector2Int imageDim;
    public int regionAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalInteger(Shader.PropertyToID("_RegionAmount"), regionAmount);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector4[] centroids = GenerateDiagram();
            // Shader.SetGlobalVectorArray("_Centroids", centroids);
            Vector4 centroid = new Vector4(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0, 0);
            Shader.SetGlobalVector(Shader.PropertyToID("_Centroid"), centroid);
        }
    }

    Vector4[] GenerateDiagram()
    {
        Vector4[] centroids = new Vector4[regionAmount];
        
        for (int i = 0; i < regionAmount; i++)
        {
            // centroids[i] = new Vector4(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y), 0, 0);
            centroids[i] = new Vector4(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0, 0);
        }

        return centroids;
    }
}
