using UnityEngine;
using System;

using Base;

public class ExplorationIcon : BaseScreenItem
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
	
	public static ExplorationIcon Create()
	{
		ExplorationIcon icon = ResourceManager.Instance.LoadAsset<ExplorationIcon>(ObjectType.GameObject, "UI/Items/ExplorationIcon");
		icon.RectTransform.parent = RootTransform.Instance.UIIconRoot;
		icon.RectTransform.localScale = Vector3.one;
		return icon;
	}
	
	public static void Recycle(ExplorationIcon icon)
	{
		if (icon != null)
		{
			icon.CallbackClick = null;
			ResourceManager.Instance.RecycleAsset(icon.gameObject);
		}
		else
		{
			Debug.Log("Recyle a null ExplorationIcon!");
		}
	}
}

