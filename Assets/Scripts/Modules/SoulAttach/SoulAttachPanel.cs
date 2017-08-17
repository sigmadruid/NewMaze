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

    public AttributeItem tempAttribute;
    public GridLayoutGroup gridAttributes;
    private UIItemPool<AttributeItem> poolAttrItem = new UIItemPool<AttributeItem>();
    public Button buttonSoulAttach;

    public SoulAttachItem tempSoulAttach;
    public GridLayoutGroup gridSoulAttach;
    private UIItemPool<SoulAttachItem> poolSoulAttach = new UIItemPool<SoulAttachItem>();

    private PackProxy packProxy;

    public override void OnInitialize()
    {
        base.OnInitialize();

        packProxy = ApplicationFacade.Instance.RetrieveProxy<PackProxy>();

        UILocalizer.LocalizeByName(transform);

        poolAttrItem.Init(tempAttribute.gameObject, gridAttributes.transform);
        poolSoulAttach.Init(tempSoulAttach.gameObject, gridSoulAttach.transform);

        ClickEventTrigger.Get(buttonClose.gameObject).onClick = OnClose;
    }

    public override void OnDispose()
    {
        base.OnDispose();

        ClickEventTrigger.Get(buttonClose.gameObject).onClick = null;
    }

    public void SetData()
    {
        List<ItemInfo> soulList = packProxy.GetItemInfosByType(ItemType.Soul);
        poolSoulAttach.RemoveAll();
        for(int i = 0; i < soulList.Count; ++i)
        {
            ItemInfo info = soulList[i];
            for(int j = 0; j < info.Count; ++j)
            {
                SoulAttachItem item = poolSoulAttach.AddItem();
                item.SetData(info.Data);
            }
        }
    }

    private void OnClose(GameObject go)
    {
        PopupManager.Instance.RemovePopup(this);
    }
}

