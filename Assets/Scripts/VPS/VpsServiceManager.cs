using System;
using System.Collections;
using System.Collections.Generic;
using naviar.ARNavigation.UI;
using naviar.VPSService;
using UnityEngine;

// VPS Wrapper for AR Navigation
public class VpsServiceManager : MonoBehaviour
{
    [SerializeField]
    private VPSLocalisationService VPS;
    [SerializeField]
    private VpsPushMessage pushMessage;

    public event System.Action OnLocalized;
    public event System.Action OnFail;
    public event System.Action<bool> OnCorrectAngle;

    private bool isWorking = false;

    private void Awake()
    {
        VPS.OnErrorHappend += OnErrorHappend;
        VPS.OnPositionUpdated += OnPositionUpdated;
        VPS.OnCorrectAngle += OnVpsCorrectAngle;
    }

    private void OnErrorHappend(ErrorInfo error)
    {
        if (error.Code == ErrorCode.NO_INTERNET)
            Toast.Instance.Show("Bad internet connection");

        OnFail?.Invoke();
    }

    private void OnPositionUpdated(LocationState locationState)
    {
        OnLocalized?.Invoke();
    }

    private void OnVpsCorrectAngle(bool isCorrect)
    {
        if (!isCorrect)
            pushMessage.Show();
        else
            pushMessage.Hide();

        OnCorrectAngle?.Invoke(isCorrect);
    }

    public void StartVPS()
    {
        if (isWorking)
            return;

        VPS.StartVPS();
        isWorking = true;
    }

    public void StopVPS()
    {
        VPS.StopVps();
        isWorking = false;
        pushMessage.Hide();
    }

    public void RestartVPS()
    {
        isWorking = false;
        StartVPS();
    }

    public void ResetTracking()
    {
        if (!Application.isEditor)
        {
            VPS.ResetTracking();
        }
    }

    public bool IsLocalized()
    {
        return VPS.IsLocalized();
    }
}
