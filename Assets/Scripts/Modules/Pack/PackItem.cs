using UnityEngine;
using UnityEngine.UI;

using System;

using GameLogic;

public class PackItem : MonoBehaviour
{
    public Image imageIcon;
    public Text textCount;

    private ItemInfo info;

    public void Init()
    {
    }

    public void SetInfo(ItemInfo info)
    {
        this.info = info;
    }

    public void Dispose()
    {
    }
}

