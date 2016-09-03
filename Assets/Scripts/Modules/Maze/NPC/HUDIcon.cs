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
        EventTriggerListener.Get(gameObject).onClick = OnIconClick;
	}

	private void OnIconClick(GameObject go)
	{
		if (CallbackClick != null)
		{
			CallbackClick();
		}
	}

    public static HUDIcon Create(HUDIconType type)
	{
        HUDIcon icon = PopupManager.Instance.CreateItem<HUDIcon>(RootTransform.Instance.UIIconRoot);
        Sprite sprite = null;
        switch(type)
        {
            case HUDIconType.NPC:
                sprite = CreateSprite("Atlas/Common/npc_icon");
                break;
            case HUDIconType.Exploration:
                sprite = CreateSprite("Atlas/Common/skull_icon");
                break;
        }
        icon.ImageIcon.sprite = sprite;
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

    private static Sprite CreateSprite(string path)
    {
        Texture2D texture = Resources.Load<Texture2D>(path);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}

