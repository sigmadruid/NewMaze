using UnityEngine;

using System;

using Base;
using GameLogic;

public class NPCIcon : BaseScreenItem 
{
	public Utils.CallbackVoid CallbackClick;

	protected override void Awake()
	{
		base.Awake();

        EventTriggerListener.Get(gameObject).onClick = OnIconClick;
	}

	private void OnIconClick(GameObject go)
	{
		if (CallbackClick != null)
		{
			CallbackClick();
		}
	}

	public static NPCIcon CreateNPCIcon()
	{
        NPCIcon icon = PopupManager.Instance.CreateItem<NPCIcon>();
        icon.RectTransform.SetParent(RootTransform.Instance.UIIconRoot);
		icon.RectTransform.localScale = Vector3.one;
		return icon;
	}
	
	public static void RecycleNPCIcon(NPCIcon icon)
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

