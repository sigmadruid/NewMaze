using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using Base;
using GameLogic;

public class BarItem : ScreenItem 
{
    private Slider slider;

	protected override void Awake()
	{
		base.Awake();

        slider = GetComponent<Slider>();
		slider.value = 1f;
	}

	public void UpdateHP(int hp, int maxHP)
	{
		slider.value = hp * 1f / maxHP;
	}

	public static BarItem CreateHPBar()
	{
		BarItem bar = ResourceManager.Instance.LoadAsset<BarItem>(ObjectType.GameObject, "NewUI/Items/BarItem");
        bar.CachedTransform.SetParent(RootTransform.Instance.UIIconRoot);
		bar.CachedTransform.localScale = Vector3.one;
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
