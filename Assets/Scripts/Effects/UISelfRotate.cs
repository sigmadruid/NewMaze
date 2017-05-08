using UnityEngine;
using UnityEngine.UI;

using System;

public class UISelfRotate : MonoBehaviour
{
    public float speed;
    public bool rotate;

    private RectTransform ui;

    void Awake()
    {
        ui = transform as RectTransform;
    }
    void Update()
    {
        if (rotate)
            ui.localEulerAngles += Vector3.forward * speed * Time.deltaTime;
    }
}

