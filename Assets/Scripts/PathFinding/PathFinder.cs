using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// Build and visualize the route 
public class PathFinder : MonoBehaviour
{
    public float MaxCornersDistance;
    public float MinCornersDistance;
    public GameObject Arrow;
    public GameObject FinalPin;

    public event System.Action OnPathfindingFailed;
    public event System.Action<Vector3[], PlanPoint> OnPathCreated;
    public event System.Action OnPathDeleted;

    public Vector3[] BuildPath(PlanPoint destinationPoint, Vector3 playerPosition)
    {
        if (destinationPoint == null)
            return null;

        ClearPath();

        NavMeshHit hit;

        NavMesh.SamplePosition(playerPosition, out hit, 100.0f, NavMesh.AllAreas);
        var startPoint = hit.position;

        NavMesh.SamplePosition(destinationPoint.GetPosition(), out hit, 100.0f, NavMesh.AllAreas);
        var endPoint = hit.position;

        var forwardPath = new NavMeshPath();
        NavMesh.CalculatePath(startPoint, endPoint, NavMesh.AllAreas, forwardPath);

        if (forwardPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError("Floor path building error! Point " + destinationPoint.PointID + " is unreachable");
            OnPathfindingFailed?.Invoke();
            return null;
        }

        List<Vector3> corners = forwardPath.corners.ToList();
        CreateAdditionalCorners(corners, MaxCornersDistance);
        DeleteClosedCorners(corners, MinCornersDistance);

        OnPathCreated?.Invoke(corners.ToArray(), destinationPoint);
        VisualPath(corners);

        return corners.ToArray();
    }

    private void VisualPath(List<Vector3> corners)
    {
        for (int i = 0; i < corners.Count; i++)
        {
            if (i != corners.Count - 1)
            {
                GameObject currentCorner = Instantiate(Arrow, corners[i], Quaternion.identity, transform);
                currentCorner.transform.LookAt(corners[i + 1]);
            }
            else
            {
                GameObject finalPin = Instantiate(FinalPin, corners[i], Quaternion.identity, transform);
                if (i != 0)
                    finalPin.transform.LookAt(corners[i - 1]);
            }
        }
    }

    private void CreateAdditionalCorners(List<Vector3> corners, float maxDistance)
    {
        for (int i = 1; i < corners.Count; i++)
        {
            if (Vector3.Distance(corners[i - 1], corners[i]) > maxDistance)
            {
                Vector3 newCorner = (corners[i - 1] + corners[i]) / 2;
                corners.Insert(i, newCorner);
                i--;
            }
        }
    }

    private void DeleteClosedCorners(List<Vector3> corners, float minDistance)
    {
        for (int i = 1; i < corners.Count; i++)
        {
            if (Vector3.Distance(corners[i - 1], corners[i]) < minDistance)
            {
                Vector3 avgPoint = (corners[i - 1] + corners[i]) / 2f;
                corners.RemoveAt(i);
                corners.RemoveAt(i - 1);
                corners.Insert(i - 1, avgPoint);
            }
        }
    }

    public void ClearPath()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        OnPathDeleted?.Invoke();
    }
}
