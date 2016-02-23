using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using Base;
using GameLogic;

public class BarItem : BaseScreenItem 
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
        BarItem bar = PopupManager.Instance.CreateItem<BarItem>();
        bar.RectTransform.SetParent(RootTransform.Instance.UIIconRoot);
		bar.RectTransform.localScale = Vector3.one;
		return bar;
	}

	public static void RecycleHPBar(BarItem bar)
	{
		if (bar != null)
		{
            PopupManager.Instance.RemoveItem(bar.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null HPBar!");
		}
	}
}
