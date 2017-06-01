using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using GameLogic;

public enum HUDIconType
{
    NPC,
    Exploration,
}

public class HUDIcon : BaseScreenItem 
{
    public Action CallbackClick;

    [HideInInspector]
    public Image ImageIcon;

	protected override void Awake()
	{
		base.Awake();

        ImageIcon = GetComponent<Image>();
        ClickEventTrigger.Get(gameObject).onClick = OnIconClick;
	}

	private void OnIconClick(GameObject go)
	{
		if (CallbackClick != null)
		{
			CallbackClick();
		}
	}

    public static HUDIcon Create(HUDIconType type, Action callbackClick)
	{
        HUDIcon icon = PopupManager.Instance.CreateItem<HUDIcon>(RootTransform.Instance.UIIconRoot);
        Sprite sprite = null;
        switch(type)
        {
            case HUDIconType.NPC:
                sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_COMMON, "npc_icon");
                break;
            case HUDIconType.Exploration:
                sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_COMMON, "skull_icon");
                break;
        }
        icon.ImageIcon.sprite = sprite;
        icon.CallbackClick = callbackClick;
		return icon;
	}
	
	public static void Recycle(HUDIcon icon)
	{
		if (icon != null)
		{
            icon.CallbackClick = null;
            PopupManager.Instance.RemoveItem(icon.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null NPCIcon!");
		}
	}


}

