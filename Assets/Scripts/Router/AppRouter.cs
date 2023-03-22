using System.Collections;
using System.Collections.Generic;
using System.Linq;
using naviar.ARNavigation.UI;
using naviar.VPSService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// API for Main Scene UI states of app
[RequireComponent(typeof(UIStateSwitcherManager))]
public class AppRouter: MonoBehaviour
{
    private static string sceneName = "Main";
    private static AppRouter instance;
    public static AppRouter Instance
    {
        get
        {
            if (SceneManager.GetActiveScene().name != sceneName)
                SceneManager.LoadScene(sceneName);
            return instance;
        }
    }

    private UIStateSwitcherManager stateSwitcher;
    [SerializeField]
    private DatabaseWrapper databaseWrapper;
    [SerializeField]
    private VpsServiceManager vpsService;

    private AppModel appModel;

    #region Initialization 
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

    private void Start()
    {
        Initialize();
        SwitchToChoosePoint();
    }

    private void Initialize()
    {
        stateSwitcher = GetComponent<UIStateSwitcherManager>();
        appModel = AppModel.Instance;
        ARNavigationSession.Instance.OnTrackingLost += OnTrackingLostHandler;
    }
    #endregion

    #region States
    public void SwitchToChoosePoint(System.Action OnChoosePointCallback = null, System.Action OnCancelCallback = null)
    {
        appModel.DeletePath();
        vpsService.StopVPS();

        List<PlanPoint> points = databaseWrapper.GetAllPoint();

        var choosePoint = stateSwitcher.GetState<ChoosePointState>();
        choosePoint.Init(points, OnChoosePointCallback, OnCancelCallback);
        stateSwitcher.SwitchState(choosePoint);
    }

    public void SwitchToScan(System.Action OnLocalizeCallback = null, System.Action OnCancelCallback = null)
    {
        appModel.DeletePath();

        vpsService.ResetTracking();
        vpsService.RestartVPS();

        var scanState = stateSwitcher.GetState<ScanState>();
        scanState.Init(OnLocalizeCallback, OnCancelCallback);
        stateSwitcher.SwitchState(scanState);
    }

    public void SwitchToWalk(System.Action OnCancelCallback = null, System.Action OnPathFindingFailCallback = null, System.Action OnPointReachedCallback = null)
    {
        AppModel.Instance.BuildPath();

        if (appModel.GetCurrentPath() == null)
        {
            Debug.LogError("The path was not build. Use AppModel.Instance.BuildPath() before");
            if (OnPathFindingFailCallback == null)
            {
                SwitchToScan();
            }
            else
            {
                OnPathFindingFailCallback();
            }

            return;
        }

        vpsService.StartVPS();

        var walkState = stateSwitcher.GetState<WalkToState>();
        walkState.Init(appModel.GetTargetPoint().PointID, OnCancelCallback, OnPathFindingFailCallback, OnPointReachedCallback);
        stateSwitcher.SwitchState(walkState);
    }

    public void SwitchToArrival(System.Action OnOkCallback = null)
    {
        appModel.DeletePath();
        vpsService.StopVPS();

        if (appModel.GetTargetPoint() == null)
        {
            Debug.LogError("The target point is not set. Use AppModel.Instance.SetTargetPoint() before");
            return;
        }

        var arrivalState = stateSwitcher.GetState<ArrivalState>();
        arrivalState.Init(appModel.GetTargetPoint(), OnOkCallback);
        stateSwitcher.SwitchState(arrivalState);
    }

    public void SwitchToRestore(System.Action OnRestoreCallback = null, System.Action OnCancelCallback = null)
    {
        appModel.DeletePath();
        vpsService.StopVPS();

        if (appModel.GetTargetPoint() == null)
        {
            Debug.LogError("The target point is not set. Use AppModel.Instance.SetTargetPoint() before");
            return;
        }

        var restoreState = stateSwitcher.GetState<RouteRestoreState>();
        restoreState.Init(appModel.GetTargetPoint(), OnRestoreCallback, OnCancelCallback);
        stateSwitcher.SwitchState(restoreState);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnTrackingLostHandler()
    {
        if (Permissions.CheckCameraPermission() != AuthorizationStatus.AUTHORIZED || Permissions.CheckLocationPermission() != AuthorizationStatus.AUTHORIZED)
        {
            PermissionAppRouter.Instance.SwitchToLoading();
            return;
        }

        if (appModel.GetCurrentPath() == null)
            SwitchToChoosePoint();
        else
            SwitchToRestore();
    }
    #endregion
}
