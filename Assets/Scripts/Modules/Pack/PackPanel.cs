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

    private ItemInfo selectedInfo;

    private UIItemPool<Toggle> togglePool = new UIItemPool<Toggle>();
    private UIItemPool<PackItem> itemPool = new UIItemPool<PackItem>();

    public override void OnInitialize()
    {
        base.OnInitialize();

        UILocalizer.LocalizeByName(transform);

        togglePool.Init(ToggleItemTypeTemplate.gameObject, GridItemType.transform);
        itemPool.Init(ItemTemplate.gameObject, GridItems.transform);

        ImageItemIcon.gameObject.SetActive(false);
        TextItemName.text = "";
        TextItemDesc.text = "";
        Array typeArr = Enum.GetValues(typeof(ItemType));
        foreach(var type in typeArr)
        {
            if((ItemType)type == ItemType.None)
                continue;
            Toggle toggle = togglePool.AddItem();
            toggle.name = TextDataManager.Instance.GetData("item.type." + type.ToString().ToLower());
            toggle.GetComponentInChildren<Text>().text = toggle.name;
        }

        ClickEventTrigger.Get(ButtonClose.gameObject).onClick = OnClose;
        ClickEventTrigger.Get(ButtonUse.gameObject).onClick = OnUse;
        ClickEventTrigger.Get(ButtonDiscard.gameObject).onClick = OnDiscard;
    }

    public override void OnDispose()
    {
        base.OnDispose();

        ClickEventTrigger.Get(ButtonClose.gameObject).onClick = null;
        ClickEventTrigger.Get(ButtonUse.gameObject).onClick = null;
        ClickEventTrigger.Get(ButtonDiscard.gameObject).onClick = null;
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
            ClickEventTrigger.Get(item.gameObject).onClick = OnSelect;
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
    private void OnSelect(GameObject go)
    {
        selectedInfo =  go.GetComponent<PackItem>().ItemInfo;
        Utils.SetActive(ImageItemIcon.gameObject, true);
        ImageItemIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_ITEM, selectedInfo.Data.Res2D);
        TextItemName.text = TextDataManager.Instance.GetData(selectedInfo.Data.Name);
        TextItemDesc.text = TextDataManager.Instance.GetData(selectedInfo.Data.Description);
    }
    private void OnUse(GameObject go)
    {
        if(selectedInfo != null)
        {
            CallbackUseItem(selectedInfo);
        }
    }
    private void OnDiscard(GameObject go)
    {
        if(selectedInfo != null)
        {
            CallbackDiscardItem(selectedInfo);
        }
    }
}

