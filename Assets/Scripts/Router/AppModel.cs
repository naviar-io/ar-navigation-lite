using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// API for logic of app
public class AppModel : MonoBehaviour
{
    private static string sceneName = "Main";
    private static AppModel instance;
    public static AppModel Instance
    {
        get
        {
            if (SceneManager.GetActiveScene().name != sceneName)
                SceneManager.LoadScene(sceneName);
            return instance;
        }
    }

    [SerializeField]
    private DatabaseWrapper databaseWrapper;
    [SerializeField]
    private PathFinder pathFinder;
    [SerializeField]
    private ARTracking arTracking;
    [SerializeField]
    private VpsServiceManager VPS;

    private Vector3 playerPosition
    {
        get
        {
            return arTracking.GetPlayerPosition();
        }
    }

    private PlanPoint targetPoint;
    private Vector3[] currentPath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            sceneName = SceneManager.GetActiveScene().name;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTargetPoint(string pointId)
    {
        PlanPoint point = databaseWrapper.GetPointById(pointId); ;
        if (point == null)
        {
            Debug.LogErrorFormat("Point with id = {0} not found", pointId);
            return;
        }
        targetPoint = point;
    }

    public bool BuildPath()
    {
        if (!VPS.IsLocalized())
        {
            Debug.LogError("You are not localized. Use VPS.StartVPS() and wait for a success server response");
            return false;
        }

        if (targetPoint == null)
        {
            Debug.LogError("The target point is not set. Use SetTargetPoint before");
            return false;
        }

        currentPath = pathFinder.BuildPath(targetPoint, playerPosition);

        return currentPath != null;
    }

    public void DeletePath()
    {
        currentPath = null;
        pathFinder.ClearPath();
    }

    public PlanPoint GetTargetPoint()
    {
        return targetPoint;
    }

    public Vector3[] GetCurrentPath()
    {
        return currentPath;
    }
}
