using UnityEngine;

using System;

using Base;
using GameLogic;

public class NPCIcon : ScreenItem 
{
	public Utils.CallbackVoid CallbackClick;

	protected override void Awake()
	{
		base.Awake();

		UIEventListener.Get(gameObject).onClick = OnIconClick;
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
		NPCIcon icon = ResourceManager.Instance.LoadAsset<NPCIcon>(ObjectType.GameObject, "UI/Items/NPCIcon");
		icon.CachedTransform.parent = RootTransform.Instance.UIIconRoot;
		icon.CachedTransform.localScale = Vector3.one;
		return icon;
	}
	
	public static void RecycleNPCIcon(NPCIcon icon)
	{
		if (icon != null)
		{
			icon.CallbackClick = null;
			ResourceManager.Instance.RecycleAsset(icon.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null NPCIcon!");
		}
	}
}

