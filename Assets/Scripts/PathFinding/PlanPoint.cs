using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The point of interest class
public class PlanPoint : MonoBehaviour
{
    public string PointID;
    public Texture2D Icon;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

#if UNITY_EDITOR
    private readonly Vector3 heightOffset = new Vector3(0, 2, 0);
    private readonly Vector3 textOffset = new Vector3(0, 0, 0);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 1f);

        string pointInfo = PointID;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        UnityEditor.Handles.Label(transform.position + heightOffset + textOffset, pointInfo, style);
    }

    private void OnValidate()
    {
        name = PointID;
    }
#endif
}
