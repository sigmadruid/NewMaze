using UnityEngine;

using System;

public class WeaponScript : MonoBehaviour
{
    private Vector3 position;
    private Vector3 eulerAngles;
    private Vector3 scale;

    void Awake()
    {
        position = transform.localPosition;
        eulerAngles = transform.localEulerAngles;
        scale = transform.localScale;
    }

    public void Attach(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.localEulerAngles = eulerAngles;
        transform.localScale = scale;
    }
}

