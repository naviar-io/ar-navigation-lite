using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointButtonUI : MonoBehaviour
{
    [SerializeField]
    private Text Name;
    [SerializeField]
    private Image Icon;

    public void SetName(string name)
    {
        Name.text = name;
    }

    public void SetIcon(Sprite icon)
    {
        Icon.sprite = icon;
    }

    public void AddListener(System.Action action)
    {
        GetComponent<Button>().onClick.AddListener(() => action());
    }
}
