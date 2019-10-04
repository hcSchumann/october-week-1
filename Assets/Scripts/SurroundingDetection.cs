using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingDetection : MonoBehaviour
{
    [SerializeField] private float sweepAngleStep = 0.1f;
    [SerializeField] private float detectionDistanceLimit = 10f;
    [SerializeField] private GameObject detectedPointPrefab;

    private Vector3 previousPosition;

    [SerializeField] List<GameObject> surroundingCorners;
    [SerializeField] List<GameObject> surroundingObjects;

    private void SweepSurrounding()
    {
        if (surroundingCorners != null)
        {
            foreach (var corner in surroundingCorners) Destroy(corner);
        }
        surroundingCorners = new List<GameObject>();
        surroundingObjects = new List<GameObject>();

        var startDirection = transform.forward;
        var firstIteration = true;

        var direction = startDirection;
        var position = transform.position;

        GameObject lastHittedObject = null;
        Vector3 lastHittedPoint = Vector3.zero;

        var iterations = Mathf.CeilToInt(360f / sweepAngleStep);
        for (int i = 0; i < iterations; i++)
        {
            var hit = new RaycastHit();
            GameObject hitedObject = null;
            if (Physics.Raycast(position, direction, out hit, detectionDistanceLimit))
            {
                hitedObject = hit.transform.gameObject;
                if (!surroundingObjects.Contains(hitedObject))
                {
                    surroundingObjects.Add(hitedObject);
                }
                lastHittedPoint = hit.point;
            }

            if (!firstIteration && hitedObject != lastHittedObject)
            {
                surroundingCorners.Add(Instantiate(detectedPointPrefab, lastHittedPoint, new Quaternion()));
            }
            lastHittedObject = hitedObject;

            direction = Quaternion.Euler(0, sweepAngleStep, 0) * direction;
            firstIteration = false;
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

        previousPosition = transform.position;
    }
}
