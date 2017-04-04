using UnityEngine;

using System;
using XftWeapon;

public class WeaponScript : MonoBehaviour
{
    public XWeaponTrail Trail;

    private Vector3 position;
    private Vector3 eulerAngles;
    private Vector3 scale;

    void Awake()
    {
        position = transform.localPosition;
        eulerAngles = transform.localEulerAngles;
        scale = transform.localScale;

        if(Trail != null)
        {
            Trail.Init();
            Trail.Deactivate();
        }
    }

    public void Attach(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.localEulerAngles = eulerAngles;
        transform.localScale = scale;
    }

    public bool TrailEnabled
    {
        set
        {
            if(Trail != null)
            {
                if(value)
                    Trail.Activate();
                else
                    Trail.Deactivate();
            }
        }
    }
}

