using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Сheck path progress, reaching the point and the need to rebuild path
public class FloorNavigationProgress : MonoBehaviour
{
    public float radiusToRebuild = 5f;
    public float pointRadiusReached = 3f;

    private Vector3[] currentPath;
    private PlanPoint destinationPoint;

    public bool CheckRebuild(Vector3 playerPosition)
    {
        if (currentPath == null)
            return false;

        playerPosition.y = currentPath[0].y;

        var nearestPoint = currentPath.OrderBy(p => Vector3.Distance(playerPosition, p)).FirstOrDefault();
        var dist = Vector3.Distance(playerPosition, nearestPoint);

        return dist > radiusToRebuild;
    }

    public bool CheckReached(Vector3 playerPosition)
    {
        if (currentPath == null)
            return false;

        playerPosition.y = currentPath[0].y;

        var finalPoint = currentPath.Last();
        var dist = Vector3.Distance(playerPosition, finalPoint);

        return dist <= pointRadiusReached;
    }

    public float GetDistanceToTarget(Vector3 playerPosition)
    {
        if (currentPath == null)
            return float.NaN;

        playerPosition.y = currentPath[0].y;

        var nearestPointIndex = Array.IndexOf(currentPath, currentPath.OrderBy(p => Vector3.Distance(playerPosition, p)).FirstOrDefault());
        nearestPointIndex = nearestPointIndex + 1 < currentPath.Length ? nearestPointIndex + 1 : nearestPointIndex;
        var nearestPointDist = Vector3.Distance(playerPosition, currentPath[nearestPointIndex]);

        var totalDist = nearestPointDist;
        for (int i = nearestPointIndex + 1; i < currentPath.Length; i++)
            totalDist += Vector3.Distance(currentPath[i - 1], currentPath[i]);

        return totalDist;
    }

    public float GetPathProgress(Vector3 playerPosition)
    {
        if (currentPath == null)
            return float.NaN;

        float pathLength = GetPathLenght();
        return (pathLength - GetDistanceToTarget(playerPosition)) / pathLength;
    }

    private float GetPathLenght()
    {
        float length = 0;
        for (int i = 1; i < currentPath.Length; i++)
            length += Vector3.Distance(currentPath[i - 1], currentPath[i]);

        return length;
    }

    public void SetPath(Vector3[] path, PlanPoint point)
    {
        currentPath = path;
        destinationPoint = point;
    }

    public PlanPoint GetCurrentDestination()
    {
        return destinationPoint;
    }

    public void DeletePath()
    {
        currentPath = null;
    }

    public bool PathIsAvailable()
    {
        return currentPath != null;
    }
}
