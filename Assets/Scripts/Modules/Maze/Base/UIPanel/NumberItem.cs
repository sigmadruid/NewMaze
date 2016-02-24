using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using Battle;
using GameLogic;

public class NumberItem : BaseScreenItem
{
	public bool AutoDestroy = true;

    private Text labelNumber;

//	private TweenScale tweenScale;
	private TweenAlpha tweenAlpha;

	protected override void Awake()
	{
		base.Awake();

        labelNumber = GetComponent<Text>();

//		tweenScale = GetComponent<TweenScale>();
		tweenAlpha = GetComponent<TweenAlpha>();
        tweenAlpha.AddOnFinished(OnFinished);
	}
	
	public void Show(AttackResult result)
	{
		gameObject.SetActive(true);

		labelNumber.color = result.IsCritical ? Color.red : Color.white;
//		tweenScale.from = Vector3.one * (result.IsCritical ? 5 : 3);
//		tweenScale.to = Vector3.one * (result.IsCritical ? 2 : 1);

		if (!result.IsDodge)
		{
			Show(result.Damage.ToString());
		}
		else
		{
			Show("Miss");
		}
	}
	public void Show(string text)
	{
		labelNumber.text = text;
		
//		tweenScale.tweenFactor = 0;
//		tweenScale.enabled = true;
		tweenAlpha.tweenFactor = 0;
		tweenAlpha.enabled = true;
	}

	private void OnFinished()
	{
		if (AutoDestroy)
		{
			NumberItem.Recycle(this);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	public static NumberItem Create(Vector3 worldPosition, AttackResult result)
	{
		NumberItem numberItem = Create(worldPosition);
		numberItem.Show(result);
		return numberItem;
	}
	public static NumberItem Create(Vector3 worldPosition, string text)
	{
		NumberItem numberItem = Create(worldPosition);
		numberItem.Show(text);
		return numberItem;
    }
	private static NumberItem Create(Vector3 worldPosition)
	{
        NumberItem numberItem = PopupManager.Instance.CreateItem<NumberItem>(RootTransform.Instance.UIIconRoot);
		numberItem.UpdatePosition(worldPosition + Vector3.up * 2.5f);
		return numberItem;
    }
	
	public static void Recycle(NumberItem number)
	{
		if (number != null)
		{
            PopupManager.Instance.RemoveItem(number.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null HPBar!");
		}
	}
}

