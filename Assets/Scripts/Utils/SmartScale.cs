using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartScale : MonoBehaviour
{
    public float maxDistance = 10f;
    public float targetScale = 1f;
    public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0.1f, 1f, 1f);

    private void Update()
    {
        var cameraPos = Camera.main.transform.position;
        var myPos = transform.position;

        var dist = Vector3.Distance(cameraPos, myPos);
        var clampedDist = Mathf.Clamp(dist, 0f, maxDistance);

        var t = clampedDist / maxDistance;
        var newScale = scaleCurve.Evaluate(t) * targetScale;

        transform.localScale = Vector3.one * newScale;
    }
}
