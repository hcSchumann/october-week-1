using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private bool shouldUpdateTriangleInfo;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
    }

    public void SetUpTriangle(Vector3[] vertices, int[] triangles)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        shouldUpdateTriangleInfo = true;
    }

    private void Update()
    {
        if (shouldUpdateTriangleInfo)
        {
            shouldUpdateTriangleInfo = false;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }
}
