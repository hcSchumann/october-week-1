using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingDetection : MonoBehaviour
{
    [SerializeField] private float sweepAngleStep = 0.1f;
    [SerializeField] private int intermediatePointStep = 50;
    [SerializeField] private float detectionDistanceLimit = 10f;

    public GameObject detectedPointPrefab;
    public GameObject intermediatePointPrefab;
    public GameObject trianglePrefab;

    private Vector3 previousPosition;

    private List<GameObject> surroundingCorners;
    private List<GameObject> soundAreaShapes;

    private void SweepSurrounding()
    {
        if (surroundingCorners != null)
        {
            foreach (var corner in surroundingCorners) Destroy(corner);
        }
        surroundingCorners = new List<GameObject>();

        var startDirection = transform.forward;
        var firstIteration = true;

        var direction = startDirection;
        var position = transform.position;

        GameObject lastHittedObject = null;
        Vector3 lastHittedPoint = Vector3.zero;
        int intermediatePointStepCount = 0;

        var iterations = Mathf.CeilToInt(360f / sweepAngleStep);
        for (int i = 0; i < iterations; i++)
        {
            intermediatePointStepCount++;
            var hit = new RaycastHit();

            GameObject hitedObject = null;
            Vector3 hitedPoint = Vector3.zero;
            if (Physics.Raycast(position, direction, out hit, detectionDistanceLimit))
            {
                hitedObject = hit.transform.gameObject;
                hitedPoint = hit.point;
                intermediatePointStepCount = 0;
            }

            if (!firstIteration && hitedObject != lastHittedObject)
            {
                if (lastHittedObject != null)
                {
                    surroundingCorners.Add(Instantiate(detectedPointPrefab, lastHittedPoint, new Quaternion()));
                }
                if (hitedObject != null)
                {
                    surroundingCorners.Add(Instantiate(detectedPointPrefab, hitedPoint, new Quaternion()));
                }
            }
            else if (intermediatePointStepCount == intermediatePointStep)
            {
                var intermediatePos = transform.position + direction * detectionDistanceLimit;
                surroundingCorners.Add(Instantiate(intermediatePointPrefab, intermediatePos, new Quaternion()));
                intermediatePointStepCount = 0;
            }

            lastHittedObject = hitedObject;
            lastHittedPoint = hitedPoint;

            direction = Quaternion.Euler(0, sweepAngleStep, 0) * direction;
            firstIteration = false;
        }
    }

    private void DrawSoundArea()
    {
        if (soundAreaShapes != null)
        {
            foreach (var corner in soundAreaShapes) Destroy(corner);
        }
        soundAreaShapes = new List<GameObject>();

        var surroundingCornersAmount = surroundingCorners.Count;
        var pos = transform.position;

        int[] verticesOrder = { 0, 1, 2 };
        for (int i = 0; i < surroundingCornersAmount; i++)
        {
            var triangle = Instantiate(trianglePrefab, transform.position, new Quaternion());
            soundAreaShapes.Add(triangle);

            Vector3[] triangleVertices = {
                surroundingCorners[i].transform.position - pos,
                surroundingCorners[(i + 1) % surroundingCornersAmount].transform.position - pos,
                Vector3.zero
            };
            triangle.GetComponent<Triangle>().SetUpTriangle(triangleVertices, verticesOrder);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position == previousPosition)
        {
            return;
        }

        SweepSurrounding();
        DrawSoundArea();
        previousPosition = transform.position;
    }
}
