using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidingZoneGraphics : MonoBehaviour {

    public Image[] targetImages;
    public AnimationCurve animationCurve = AnimationCurve.Constant(0, 180, 1);

    public void ShowGuidingImage()
    {
        gameObject.SetActive(true);
    }

    public void HideGuidingImage()
    {
        gameObject.SetActive(false);
    }

    public void SetImageAlphaByAngle(float angle)
    {
        foreach (var targetImage in targetImages)
        {
            var imageColor = targetImage.color;

            var angleUnsigned = Mathf.Abs(angle);
            imageColor.a = animationCurve.Evaluate(angleUnsigned);
            targetImage.color = imageColor;
        }
    }
}
