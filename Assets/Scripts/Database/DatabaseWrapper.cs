using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Database of points wrapper
public class DatabaseWrapper : MonoBehaviour
{
    private List<PlanPoint> planPoints = new List<PlanPoint>();

    void Start()
    {
        foreach(Transform child in transform)
        {
            PlanPoint planPoint = child.GetComponent<PlanPoint>();
            if (planPoint != null)
                planPoints.Add(planPoint);
        }
    }

    public List<PlanPoint> GetAllPoint()
    {
        return planPoints;
    }

    public PlanPoint GetPointById(string pointId)
    {
        return planPoints.FirstOrDefault(p => p.PointID == pointId);
    }
}
