using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public float seaBed = -5.0f;
    public float left = -50.0f;
    public float right = 50.0f;
    public float nearest = -20.0f;
    public float zPerChunk = 120.0f;
    public int verticePerUnit = 3;
    public int chunksToKeep = 3;
    public float landNoiseZoom = 10.0f;
    public float landMaxAltitude = 5.0f;
    public float landStartZ = 60.0f;
    public float edgeBorder = 4.0f;
    public float waterLineWidthFrom = 20.0f;
    public float waterLineWidthTo = 5.0f;
    public float waterLineNoiseZoom = 40.0f;

    public GameObject terrainChunkPrefab;

    public float latestGeneratedNear = 0;

    GameObject[] chunks;
    int chunkIndex = 0;

    float[] noiseBase;

    void Start()
    {
        noiseBase = new float[] { Random.Range(0.0f, 1000000.0f), Random.Range(0.0f, 1000000.0f), Random.Range(0.0f, 1000000.0f) };

        chunks = new GameObject[chunksToKeep];

        while (chunkIndex < 2) Generate();
    }

    public void Generate()
    {
        if (chunks[chunkIndex % chunksToKeep] != null) Destroy(chunks[chunkIndex % chunksToKeep]);
        var newChunk = Instantiate(terrainChunkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        chunks[chunkIndex % chunksToKeep] = newChunk;
        latestGeneratedNear = nearest + zPerChunk * chunkIndex;
        var mesh = createMesh(latestGeneratedNear, nearest + zPerChunk * (chunkIndex + 1));
        newChunk.name = $"TerrainChunk [{chunkIndex}]";
        chunkIndex++;

        newChunk.GetComponent<MeshFilter>().mesh = mesh;
        newChunk.GetComponent<MeshCollider>().sharedMesh = mesh;

        newChunk.transform.parent = this.transform;
    }

    Mesh createMesh(float near, float far)
    {
        int xSize = ((int) ((right - left) * verticePerUnit));
        int zSize = ((int) ((far - near) * verticePerUnit));
        float verticeDist = 1.0f / verticePerUnit;

        var vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float x, y, z;
        for (int j = 0; j <= zSize; j++) {
            for (int i = 0; i <= xSize; i++) {
                x = left + verticeDist * i;
                z = near + verticeDist * j;
                y = getTerrainHeight(x, z);
                vertices[j * (xSize + 1) + i] = new Vector3(x, y, z);
            }
        }

        var triangles = new int[xSize * zSize * 6];
        int triangleIndex;
        for (int j = 0; j < zSize; j++) {
            for (int i = 0; i < xSize; i++) {
                triangleIndex = (j * xSize + i) * 6;

                triangles[triangleIndex] = j * (xSize + 1) + (i + 1);
                triangles[triangleIndex + 1] = j * (xSize + 1) + i;
                triangles[triangleIndex + 2] = (j + 1) * (xSize + 1) + i;
                triangles[triangleIndex + 3] = j * (xSize + 1) + (i + 1);
                triangles[triangleIndex + 4] = (j + 1) * (xSize + 1) + i;
                triangles[triangleIndex + 5] = (j + 1) * (xSize + 1) + (i + 1);
            }
        }
        
        var mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    float getTerrainHeight(float x, float z)
    {
        float land = Mathf.PerlinNoise(
            (x + noiseBase[0] - left) / landNoiseZoom,
            (z + noiseBase[1] - nearest) / landNoiseZoom
        ) * (landMaxAltitude - seaBed);

        float landReduceFactor = 1.0f;
        if (x <= (left + edgeBorder))
        {
            landReduceFactor *= Mathf.SmoothStep(0, 1, (x - left) / edgeBorder);
        }
        if (x >= (right - edgeBorder))
        {
            landReduceFactor *= Mathf.SmoothStep(0, 1, (right - x) / edgeBorder);
        }

        float waterLineAtX = Mathf.PerlinNoise(
            (z + noiseBase[2] - nearest) / waterLineNoiseZoom, 0
        ) * (right - left) + left;
        if (z <= landStartZ) waterLineAtX *= Mathf.SmoothStep(1, 0, (landStartZ - z) / ((landStartZ - nearest) / 2));
        float waterLineWidth = Mathf.SmoothStep(waterLineWidthFrom, waterLineWidthTo, (nearest - z) / (landStartZ - nearest));
        landReduceFactor *= Mathf.SmoothStep(0, 1, Mathf.Abs(x - waterLineAtX) / waterLineWidth);

        //return seaBed + landReduceFactor * (landMaxAltitude - seaBed); // debug landReduceFactor
        return seaBed + landReduceFactor * land;
    }
}
