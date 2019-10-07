using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePool : MonoBehaviour
{
    [SerializeField] private GameObject trianglePrefab;

    private List<PoolTriangle> trianglePool = new List<PoolTriangle>();

    private int availableTriangles = 0;
    private int nextAvailablePoolIndex = 0;

    private int[] verticesOrder = { 0, 1, 2 };

    private struct PoolTriangle
    {
        public GameObject triangleObj;
        public Triangle triangle;
    }

    public void ResetTrianglePool()
    {
        availableTriangles = trianglePool.Count;
        nextAvailablePoolIndex = 0;

        foreach (var poolTriangle in trianglePool)
        {
            poolTriangle.triangle.SetIsVisible(false);
        }
    }

    public void ClearPool()
    {
        foreach (var triangle in trianglePool)
        {
            Destroy(triangle.triangleObj);
        }
        trianglePool = new List<PoolTriangle>();
    }

    private void InstantiateTriangle()
    {
        var triangleObj = Instantiate(trianglePrefab, transform);
        var poolTriangle = new PoolTriangle
        {
            triangleObj = triangleObj,
            triangle = triangleObj.GetComponent<Triangle>()
        };
        trianglePool.Add(poolTriangle);
        availableTriangles++;
    }

    private PoolTriangle GetAvailablePoolTriangle()
    {
        if (availableTriangles == 0) InstantiateTriangle();

        availableTriangles--;
        return trianglePool[nextAvailablePoolIndex++];
    }

    public void DrawTriangle(Vector3 pos, Vector3[] vertices)
    {
        var poolTriangle = GetAvailablePoolTriangle();
        poolTriangle.triangleObj.transform.position = pos;

        poolTriangle.triangle.SetUpTriangle(vertices, verticesOrder);
    }
}
