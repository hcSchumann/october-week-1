using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private Color waveColor;
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private int[] triangles;

    private bool shouldUpdateTriangleInfo;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = waveColor;
        mesh.Clear();
    }

    public void SetUpTriangle(Vector3[] vertices, int[] triangles)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        shouldUpdateTriangleInfo = true;
        SetIsVisible(true);
    }

    public void SetIsVisible(bool isVisible)
    {
        meshRenderer.enabled = isVisible;
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
