using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

public class PackPanel : BasePopupView
{
    [Header("Top Part")]
    public GridLayoutGroup GridItemType;
    public Toggle ToggleItemTypeTemplate;

    [Header("Left Part")]
    public Image ImageItemIcon;
    public Text TextItemName;
    public Text TextItemDesc;
    public Button ButtonUse;
    public Button ButtonDiscard;

    [Header("Right Part")]
    public GridLayoutGroup GridItems;
    public PackItem ItemTemplate;

    public override void onInitialize()
    {
        base.onInitialize();
    }

    public override void onDispose()
    {
        base.onDispose();
    }
}

