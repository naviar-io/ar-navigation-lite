using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VpsPushMessage : MonoBehaviour
{
    public bool Active = false;
    private bool preShown = false;

    public string ShowAnimName, HideAnimName;

    public float TimeToShow = 3;

    public void Show()
    {
        if (Active || preShown)
            return;

        preShown = true;
        StartCoroutine(ShowWithDelay());
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(TimeToShow);
        if (!preShown)
            yield break;

        GetComponent<Animation>().Play(ShowAnimName);
        Active = true;
        preShown = false;
    }

    public void Hide()
    {
        StopAllCoroutines();
        preShown = false;

        if (!Active)
            return;

        GetComponent<Animation>().Play(HideAnimName);
        Active = false;
    }
}
