using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utilities
{
    public static Vector3 GetPointByRayCast(Vector3 point, bool drawRay = false)
    {
        Vector3 hitPoint = point;
        point.y = 200f;
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
        Vector3 randomPos = EnviromentManager.instance.GetRandomPosInArea();

        return GetPointByRayCast(randomPos);
    }

    //public static Vector3 GetLocation(Vector3 point, bool drawRay = false)
    //{
    //    NavMesh.SamplePosition(point, out var hit, 100f, 1);
    //    Vector3 finalPosition = hit.position;
    //    if (drawRay)
    //        Debug.DrawRay(finalPosition, Vector3.up, Color.blue, 1.0f);
    //    return finalPosition;
    //}

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

    private static Vector3 newDir = Vector3.zero;
    public static Vector3 GetDirectionByWall(Wall wall, Vector3 targetDirection)
    {
        if (wall == null)
            return targetDirection;

        switch (wall.BuildingSide)
        {
            case BuildingSide.Front:
                {
                    newDir.z = 0f;
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z;
                }
                return newDir;
            case BuildingSide.Right:
                {
                    newDir.x = 0f;
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x * -1f;
                }
                return newDir;
            case BuildingSide.Back:
                {
                    newDir.z = 0f;
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z * -1f;
                }
                return newDir;
            case BuildingSide.Left:
                {
                    newDir.x = 0f;
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x;
                }
                return newDir;
            case BuildingSide.Top:
            case BuildingSide.Ground:
            default:
                {
                    newDir.y = 0f;
                    newDir.z = targetDirection.z;
                    newDir.x = targetDirection.x;
                }
                return newDir;
        }
    }

    public static bool LinesIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
}
