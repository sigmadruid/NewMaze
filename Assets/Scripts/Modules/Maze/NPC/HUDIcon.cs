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
    [HideInInspector]
    public Image ImageIcon;

	protected override void Awake()
	{
		base.Awake();

        ImageIcon = GetComponent<Image>();
	}

    public static HUDIcon Create(HUDIconType type)
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
		return icon;
	}
	
	public static void Recycle(HUDIcon icon)
	{
		if (icon != null)
		{
            PopupManager.Instance.RemoveItem(icon.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null NPCIcon!");
		}
	}


}

