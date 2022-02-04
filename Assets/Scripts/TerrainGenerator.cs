using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    public float yBase = -5.0f;
    public float left = -20.0f;
    public float right = 20.0f;
    public int verticePerUnit = 3;
    public float noiseZoom = 10.0f;
    public float noiseScale = 8.0f;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        CreateShape();
        CreateMesh();
    }

    void CreateShape ()
    {
        float far = 80.0f;
        float near = -20.0f;

        int xSize = ((int) ((right - left) * verticePerUnit));
        int zSize = ((int) ((far - near) * verticePerUnit));
        float verticeDist = 1.0f / verticePerUnit;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float x, y, z;
        for (int j = 0; j <= zSize; j++) {
            for (int i = 0; i <= xSize; i++) {
                x = left + verticeDist * i;
                z = near + verticeDist * j;
                y = yBase + Mathf.PerlinNoise(
                    x / noiseZoom - left,
                    z / noiseZoom - near
                ) * noiseScale;
                vertices[j * (xSize + 1) + i] = new Vector3(x, y, z);
            }
        }

        triangles = new int[xSize * zSize * 6];
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
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnDrawGizmos()
    //{
        //if (vertices == null) return;

        //for (int i = 0; i < vertices.Length; i++) {
            //Gizmos.DrawSphere(vertices[i], .1f);
        //}
    //}
}
