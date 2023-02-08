using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathProgressView : MonoBehaviour
{
    public RectTransform rectTransform;

    public void SetProgress(float percent)
    {
        rectTransform.anchorMax = new Vector2(percent + 0.05f, 1f);
    }
}
