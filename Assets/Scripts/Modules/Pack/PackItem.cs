using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using GameLogic;

public class PackItem : MonoBehaviour
{
    public Image ImageIcon;
    public Text TextCount;

    public ItemInfo ItemInfo { get; private set; }

    public void Init()
    {
    }

    public void SetInfo(ItemInfo info)
    {
        this.ItemInfo = info;
        TextCount.text = info.Count.ToString();
        ImageIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_ITEM, info.Data.Res2D);
    }

    public void Dispose()
    {
    }
}

