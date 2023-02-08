using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnRun : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
