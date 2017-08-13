using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class SoulAttachPanel : BasePopupView
{
    public Button buttonClose;
    public UITargetTexture texWeapon;
    public Button buttonLeft;
    public Button buttonRight;

    public AttributeItem AttributeItemPrefab;
    public GridLayoutGroup gridAttributes;
    private UIItemPool<AttributeItem> poolAttrItem = new UIItemPool<AttributeItem>();
    public Button buttonSoulAttach;

    public SoulAttachItem tempSoulAttach;
    public GridLayoutGroup gridSoulAttach;
    private UIItemPool<SoulAttachItem> poolSoulAttach = new UIItemPool<SoulAttachItem>();

    public override void OnInitialize()
    {
        base.OnInitialize();

        UILocalizer.LocalizeByName(transform);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }
}

