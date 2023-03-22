using System.Collections;
using System.Collections.Generic;
using naviar.ARNavigation.UI;
using UnityEngine;

// Class to register AR Navigation session events
[RequireComponent(typeof(FloorNavigationProgress))]
public class ARNavigationSession : MonoBehaviour
{
    private static ARNavigationSession instance;
    public static ARNavigationSession Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private VpsServiceManager vpsService;
    [SerializeField]
    private PathFinder pathFinder;
    [SerializeField]
    private ARTracking arTracking;

    private FloorNavigationProgress floorProgress;

    // session was interrupted 
    public event System.Action OnTrackingLost;
    // can't build route between current position and chosen point
    public event System.Action OnPathFindingFailed;
    // route was built successfully
    public event System.Action<Vector3[], PlanPoint> OnPathCreated;
    // route was deleted 
    public event System.Action OnPathDeleted;

    // user reached the chosen point
    public event System.Action<PlanPoint> OnPointReached;
    // path progress updated 
    public event System.Action<float, float> OnProgressUpdated;

    // camera angle became correct / incorrect for VPS work
    public event System.Action<bool> OnCorrectVpsAngle;
    // receive success localization from VPS server
    public event System.Action OnVpsRequestSuccess;
    // receive fail localization from VPS server
    public event System.Action OnVpsRequestFailed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    private void Initialize()
    {
        arTracking.OnTrackingLost += OnTrackingLostHandler;

        pathFinder.OnPathfindingFailed += OnPathFindingFailedHandler;
        pathFinder.OnPathCreated += OnPathCreatedHandler;
        pathFinder.OnPathDeleted += OnPathDeletedHandler;

        vpsService.OnLocalized += () => OnVpsRequestSuccess?.Invoke();
        vpsService.OnFail += () => OnVpsRequestFailed?.Invoke();
        vpsService.OnCorrectAngle += (correct) => OnCorrectVpsAngle?.Invoke(correct);

        floorProgress = GetComponent<FloorNavigationProgress>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        if (floorProgress.PathIsAvailable())
        {
            Vector3 playerPos = GetCurrentPlayerPosition();
            if (floorProgress.CheckRebuild(playerPos))
                pathFinder.BuildPath(floorProgress.GetCurrentDestination(), playerPos);
            else if (floorProgress.CheckReached(playerPos))
            {
                OnPointReached?.Invoke(floorProgress.GetCurrentDestination());
                floorProgress.DeletePath();
            }

            OnProgressUpdated?.Invoke(floorProgress.GetPathProgress(playerPos), floorProgress.GetDistanceToTarget(playerPos));
        }
    }

    private void OnPathCreatedHandler(Vector3[] floorPath, PlanPoint desctination)
    {
        floorProgress.SetPath(floorPath, desctination);
        OnPathCreated?.Invoke(floorPath, desctination);
    }

    private void OnPathFindingFailedHandler()
    {
        floorProgress.DeletePath();
        Toast.Instance.Show("Can't build the route from this place. Try localizing from another location");
        OnPathFindingFailed?.Invoke();
    }

    private void OnPathDeletedHandler()
    {
        floorProgress.DeletePath();
        OnPathDeleted?.Invoke();
    }

    private void OnTrackingLostHandler()
    {
        OnTrackingLost?.Invoke();
        floorProgress.DeletePath();
    }

    public Vector3 GetCurrentPlayerPosition()
    {
        return arTracking.GetPlayerPosition();
    }
}