using UnityEngine;
using System;

using GameUI;

public class Test : MonoBehaviour
{
    public BubbleItem bubble;

    void Awake()
    {
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            bubble.Show(true, "aaa", "bbb");
        }
        if(Input.GetMouseButtonDown(1))
        {
            bubble.Show(false, "aaa", "bbb");
        }
    }
}

