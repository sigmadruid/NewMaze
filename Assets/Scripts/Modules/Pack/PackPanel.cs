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
    public Action<ItemInfo> CallbackDiscardItem;

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

    public override void OnInitialize()
    {
        base.OnInitialize();

        togglePool.Init(ToggleItemTypeTemplate.gameObject, GridItemType.transform);
        itemPool.Init(ItemTemplate.gameObject, GridItems.transform);

        EventTriggerListener.Get(ButtonClose.gameObject).onClick = OnClose;
        EventTriggerListener.Get(ButtonUse.gameObject).onClick = OnClose;
        EventTriggerListener.Get(ButtonDiscard.gameObject).onClick = OnClose;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public void SetInfo(ItemType itemType, List<ItemInfo> infoList)
    {
        itemPool.RemoveAll();
        for (int i = 0; i < infoList.Count; ++i)
        {
            ItemInfo info = infoList[i];
            PackItem item = itemPool.AddItem();
            item.Init();
            item.SetInfo(info);
        }
    }

    public ItemType CurrentItemType { get; private set; }

    private void OnClose(GameObject go)
    {
        PopupManager.Instance.RemovePopup(this);
    }
    private void OnSwitchType(GameObject go)
    {
    }
    private void OnUse(GameObject go)
    {
        PackItem item = go.GetComponent<PackItem>();
        CallbackUseItem(item.ItemInfo);
    }
    private void OnDiscard(GameObject go)
    {
        PackItem item = go.GetComponent<PackItem>();
        CallbackDiscardItem(item.ItemInfo);
    }
}

