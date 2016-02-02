using UnityEngine;
using System.Collections;

using Base;
using GameLogic;

public class BarItem : ScreenItem 
{
	private UISlider slider;

	protected override void Awake()
	{
		base.Awake();

		slider = GetComponent<UISlider>();
		slider.value = 1f;
	}

	public void UpdateHP(int hp, int maxHP)
	{
		slider.value = hp * 1f / maxHP;
	}

	public static BarItem CreateHPBar()
	{
		BarItem bar = ResourceManager.Instance.LoadAsset<BarItem>(ObjectType.GameObject, "UI/Items/BarItem");
		bar.CachedTransform.parent = RootTransform.Instance.UIIconRoot;
		bar.CachedTransform.localScale = Vector3.one;
		bar.GetComponent<UIWidget>().depth = 10;
		return bar;
	}

	public static void RecycleHPBar(BarItem bar)
	{
		if (bar != null)
		{
			ResourceManager.Instance.RecycleAsset(bar.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null HPBar!");
		}
	}
}
