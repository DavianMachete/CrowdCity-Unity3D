using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utilities
{
    public static Vector3 GetPointByRayCast(Vector3 point, bool drawRay = false)
    {
        Vector3 hitPoint = point;
        point.y = 100f;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            hitPoint = hit.point;
            if (drawRay)
                Debug.DrawRay(hitPoint, Vector3.up, Color.blue, 1.0f);
        }
        return hitPoint;
    }


    public static Vector3 GetRandomLocation()
    {
        Vector3 randomPos = EnviromentManager.instance.GetRandomLocationInArea();

        return GetLocation(randomPos);
    }

    public static Vector3 GetLocation(Vector3 point, bool drawRay = false)
    {
        NavMesh.SamplePosition(point, out var hit, 100f, 1);
        Vector3 finalPosition = hit.position;
        if (drawRay)
            Debug.DrawRay(finalPosition, Vector3.up, Color.blue, 1.0f);
        return finalPosition;
    }

    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        //Get heading
        Vector3 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }
}
