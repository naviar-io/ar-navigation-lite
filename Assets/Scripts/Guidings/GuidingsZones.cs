using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidingsZones : MonoBehaviour
{
    public GuidingZoneGraphics leftImage, rightImage;
    public int moveAlong = 2;
    private Transform mainCameraTransform;

    private void Awake()
    {
        mainCameraTransform = Camera.main.transform;
        leftImage.HideGuidingImage();
        rightImage.HideGuidingImage();
    }

    void Update()
    {
        Vector3 target = ClosestPathPoint(); 

        var dir = target - mainCameraTransform.position;
        var dirPoject = Vector3.ProjectOnPlane(dir, mainCameraTransform.forward);

        var worldPoint = mainCameraTransform.position + mainCameraTransform.forward + dirPoject * 1000f;

        Vector2 finalDir = Camera.main.WorldToScreenPoint(worldPoint);
        var dirProjectNorm = finalDir.normalized;

        var worldAngle = Vector3.SignedAngle(mainCameraTransform.forward, dir, Vector3.up);
        var angle = Vector2.SignedAngle(Vector2.up, dirProjectNorm);
        bool isLeft = angle < 0;

        if (isLeft)
        {
            leftImage.HideGuidingImage();
            rightImage.ShowGuidingImage();
            rightImage.SetImageAlphaByAngle(worldAngle);
        }
        else
        {
            rightImage.HideGuidingImage();
            leftImage.ShowGuidingImage();
            leftImage.SetImageAlphaByAngle(worldAngle);
        }
    }

    private Vector3 ClosestPathPoint()
    {
        Vector3[] currentPath = AppModel.Instance.GetCurrentPath();
        if (currentPath == null)
        {
            Debug.LogError("Current path is null, can't take nearest point");
            return Vector3.zero;
        }

        Vector3 playerPosition = ARNavigationSession.Instance.GetCurrentPlayerPosition();

        int closestPointIndex = 0;
        float minDist = Vector3.Distance(playerPosition, currentPath[closestPointIndex]);
        for (int i = 1; i < currentPath.Length; i++)
        {
            var dist = Vector3.Distance(playerPosition, currentPath[i]);
            if (dist < minDist)
            {
                closestPointIndex = i;
                minDist = dist;
            }
        }

        bool isNearEnd = moveAlong + closestPointIndex < currentPath.Length;
        var closestPoint = isNearEnd ? currentPath[closestPointIndex + moveAlong] : currentPath[currentPath.Length - 1];

        Debug.DrawLine(playerPosition, closestPoint);

        return closestPoint;
    }
}
