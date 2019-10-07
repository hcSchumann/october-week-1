using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHitSorting : IComparer<RaycastHit>
{
    Vector3 originPosition;
    public RaycastHitSorting(Vector3 position)
    {
        originPosition = position;
    }

    public int Compare(RaycastHit hitA, RaycastHit hitB)
    {
        var distanceA = Vector3.Magnitude(hitA.transform.position - originPosition);
        var distanceB = Vector3.Magnitude(hitB.transform.position - originPosition);

        if (distanceA < distanceB) return -1;
        if (distanceA > distanceB) return 1;

        return 0;
    }
}
