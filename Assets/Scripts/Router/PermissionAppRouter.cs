using System.Collections;
using System.Collections.Generic;
using ARVRLab.ARNavigation.MockupV3;
using UnityEngine;
using UnityEngine.SceneManagement;

// API for Permission Scene UI states of app
[RequireComponent(typeof(UIStateSwitcherManager))]
public class PermissionAppRouter : MonoBehaviour
{
    private static string sceneName = "PermissionScene";
    private static PermissionAppRouter instance;
    public static PermissionAppRouter Instance
    {
        get
        {
            if (SceneManager.GetActiveScene().name != sceneName)
                SceneManager.LoadScene(sceneName);
            return instance;
        }
    }

    private UIStateSwitcherManager stateSwitcher;

    void Start()
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

        Initialize();
        SwitchToLoading();
    }

    private void Initialize()
    {
        stateSwitcher = GetComponent<UIStateSwitcherManager>();
    }

    public void SwitchToLoading(System.Action OnNotARSupportCallback = null, System.Action OnNotPermissionsCallback = null,
            System.Action OnSuccessCallback = null)
    {
        var loadingState = stateSwitcher.GetState<LoadingSceneState>();
        loadingState.Init(OnNotARSupportCallback, OnNotPermissionsCallback, OnSuccessCallback);
        stateSwitcher.SwitchState(loadingState);
    }

    public void SwitchToPermissive(System.Action OnContinueCallback = null)
    {
        var permissiveState = stateSwitcher.GetState<PermissiveState>();
        permissiveState.Init(OnContinueCallback);
        stateSwitcher.SwitchState(permissiveState);
    }

    public void SwitchToNotARSupport(System.Action OnCloseCallback = null)
    {
        var notArSupportState = stateSwitcher.GetState<NotArSupportState>();
        notArSupportState.Init(OnCloseCallback);
        stateSwitcher.SwitchState(notArSupportState);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
