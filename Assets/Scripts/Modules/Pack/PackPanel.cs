using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class PackPanel : BasePopupView
{
    public Action<ItemType> CallbackSwitchType;
    public Action<ItemInfo> CallbackUseItem;
    public Action<ItemInfo> CallbackDeleteItem;

    public Button ButtonClose;

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
    public ScrollRect ScrollItems;
    public GridLayoutGroup GridItems;
    public PackItem ItemTemplate;

    private UIItemPool<Toggle> togglePool = new UIItemPool<Toggle>();
    private UIItemPool<PackItem> itemPool = new UIItemPool<PackItem>();

    public override void onInitialize()
    {
        base.onInitialize();

        togglePool.Init(ToggleItemTypeTemplate.gameObject, GridItemType.transform);
        itemPool.Init(ItemTemplate.gameObject, GridItems.transform);

        EventTriggerListener.Get(ButtonClose.gameObject).onClick = OnClose;
    }

    public override void onDispose()
    {
        base.onDispose();
    }

    public void SetData(ItemType itemType, List<ItemInfo> infoList)
    {
    }

    private void OnClose(GameObject go)
    {
        PopupManager.Instance.RemovePopup(this);
    }
}

