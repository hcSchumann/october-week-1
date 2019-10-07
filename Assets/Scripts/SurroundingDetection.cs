using System;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingDetection : MonoBehaviour
{
    public bool DebugMode = false;

    [SerializeField] private string[] targetObjectTag;
    [SerializeField] private float sweepAngleStep = 0.1f;
    [SerializeField] private int intermediatePointStep = 50;
    [SerializeField] private float detectionDistanceLimit = 10f;

    public TrianglePool trianglePool;
    public GameObject detectedPointPrefab;
    public GameObject intermediatePointPrefab;

    private Vector3 previousPosition;

    private List<Vector3> surroundingCornersPoints;
    private List<GameObject> surroundingCornerObjects;

    private struct DirectionCheckResult
    {
        public bool IsEmpty;
        public GameObject lastHittedObject;
        public Vector3 lastHittedPoint;
        public int intermediatePointStepCount;
    }

    private DirectionCheckResult CheckGivenDirection(Vector3 direction, Vector3 position, DirectionCheckResult lastResult)
    {
        var intermediatePointStepCount = lastResult.intermediatePointStepCount;
        var lastHittedObject = lastResult.lastHittedObject;
        var lastHittedPoint = lastResult.lastHittedPoint;

        GameObject hitedObject = null;
        Vector3 hitedPoint = Vector3.zero;

        var hitList = Physics.RaycastAll(position, direction, detectionDistanceLimit);
        Array.Sort(hitList, new RaycastHitSorting(position));

        foreach (var hit in hitList)
        {
            if (IsATargetObject(hit.transform.gameObject))
            {
                var obj = hit.transform.gameObject;
                hitedObject = obj;
                hitedPoint = hit.point;
                intermediatePointStepCount = 0;
                break;
            }
        }

        if (!lastResult.IsEmpty && hitedObject != lastHittedObject)
        {
            if (lastHittedObject != null)
            {
                surroundingCornersPoints.Add(lastHittedPoint);
                if (DebugMode) surroundingCornerObjects.Add(Instantiate(detectedPointPrefab, lastHittedPoint, new Quaternion()));
            }
            if (hitedObject != null)
            {
                surroundingCornersPoints.Add(hitedPoint);
                if (DebugMode) surroundingCornerObjects.Add(Instantiate(detectedPointPrefab, hitedPoint, new Quaternion()));
            }
        }
        else if (intermediatePointStepCount == intermediatePointStep)
        {
            var intermediatePos = transform.position + direction * detectionDistanceLimit;
            surroundingCornersPoints.Add(intermediatePos);
            intermediatePointStepCount = 0;
            if (DebugMode) surroundingCornerObjects.Add(Instantiate(intermediatePointPrefab, intermediatePos, new Quaternion()));
        }

        lastResult.IsEmpty = false;
        lastResult.lastHittedObject = hitedObject;
        lastResult.lastHittedPoint = hitedPoint;
        lastResult.intermediatePointStepCount = intermediatePointStepCount;

        return lastResult;
    }

    private void CheckSurroundingDirections()
    {
        if (DebugMode) ClearSurroundingCornerObjects();

        surroundingCornersPoints = new List<Vector3>();

        var startDirection = transform.forward;
        var direction = startDirection;
        var position = transform.position;

        var directionCheckResult = new DirectionCheckResult
        {
            IsEmpty = true
        };

        var iterations = Mathf.CeilToInt(360f / sweepAngleStep);
        for (int i = 0; i < iterations; i++)
        {
            directionCheckResult.intermediatePointStepCount++;

            directionCheckResult = CheckGivenDirection(direction, position, directionCheckResult);

            direction = Quaternion.Euler(0, sweepAngleStep, 0) * direction;
        }
    }

    private void DrawSoundArea()
    {
        trianglePool.ResetTrianglePool();

        var surroundingCornersAmount = surroundingCornersPoints.Count;
        var pos = transform.position;

        for (int i = 0; i < surroundingCornersAmount; i++)
        {
            Vector3[] triangleVertices = {
                surroundingCornersPoints[i] - pos,
                surroundingCornersPoints[(i + 1) % surroundingCornersAmount] - pos,
                Vector3.zero
            };
            trianglePool.DrawTriangle(pos, triangleVertices);
        }
    }


    void Update()
    {
        if (transform.position == previousPosition)
        {
            return;
        }

        CheckSurroundingDirections();
        DrawSoundArea();
        previousPosition = transform.position;
    }

    private void ClearSurroundingCornerObjects()
    {
        if (surroundingCornerObjects != null)
        {
            foreach (var corner in surroundingCornerObjects) Destroy(corner);
        }
        surroundingCornerObjects = new List<GameObject>();
    }

    private bool IsATargetObject(GameObject obj)
    {
        foreach (var tag in targetObjectTag)
        {
            if (obj.tag == tag) return true;
        }
        return false;
    }


}
