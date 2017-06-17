using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using Base;
using GameLogic;
using StaticData;

public class BarItem : BaseScreenItem 
{
    [Header("Width")]
    public float SmallWidth = 40f;
    public float MediumWidth = 60f;
    public float BigWidth = 80f;

    [Header("Height")]
    public float SmallHeight = 10f;
    public float MediumHeight = 10f;
    public float BigHeight = 15f;

    private TransitionProgressBar bar;

	protected override void Awake()
	{
		base.Awake();

        bar = GetComponent<TransitionProgressBar>();
        bar.SetValue(1, false);
	}

    public void UpdateHP(int hp, int maxHP, bool withAnim)
	{
        bar.SetValue(hp * 1f / maxHP, withAnim);
	}

    private void SetSizeStyle(float width, float height)
    {
        (bar.BackBar.transform as RectTransform).sizeDelta = new Vector2(width, height);
        (bar.BackBar.transform as RectTransform).sizeDelta = new Vector2(width, height);
        (bar.ForeBar.transform as RectTransform).sizeDelta = new Vector2(width, height);
    }

    public static BarItem Create(MonsterSize size)
	{
        BarItem bar = PopupManager.Instance.CreateItem<BarItem>(RootTransform.Instance.UIIconRoot);
        switch(size)
        {
            case MonsterSize.Small:
                bar.SetSizeStyle(bar.SmallWidth, bar.SmallHeight);
                break;
            case MonsterSize.Medium:
                bar.SetSizeStyle(bar.MediumWidth, bar.MediumHeight);
                break;
            case MonsterSize.Big:
                bar.SetSizeStyle(bar.BigWidth, bar.BigHeight);
                break;
        }
		return bar;
	}

	public static void Recycle(BarItem bar)
	{
		if (bar != null)
		{
            bar.UpdateHP(1, 1, false);
            PopupManager.Instance.RemoveItem(bar.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null HPBar!");
		}
	}
}
