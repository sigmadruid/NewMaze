using UnityEngine;

using System;

using Base;
using GameLogic;

public class NumberItem : ScreenItem
{
	public bool AutoDestroy = true;

	private UILabel labelNumber;

	private TweenScale tweenScale;
	private TweenAlpha tweenAlpha;

	protected override void Awake()
	{
		base.Awake();

		labelNumber = GetComponent<UILabel>();

		tweenScale = GetComponent<TweenScale>();
		tweenAlpha = GetComponent<TweenAlpha>();
		tweenAlpha.AddOnFinished(OnFinished);
	}
	
	public void Show(AttackResult result)
	{
		gameObject.SetActive(true);

		labelNumber.color = result.isCritical ? Color.red : Color.white;
		tweenScale.from = Vector3.one * (result.isCritical ? 5 : 3);
		tweenScale.to = Vector3.one * (result.isCritical ? 2 : 1);

		if (!result.isDodge)
		{
			Show(result.damage.ToString());
		}
		else
		{
			Show("Miss");
		}
	}
	public void Show(string text)
	{
		labelNumber.text = text;
		
		tweenScale.tweenFactor = 0;
		tweenScale.enabled = true;
		tweenAlpha.tweenFactor = 0;
		tweenAlpha.enabled = true;
	}

	private void OnFinished()
	{
		if (AutoDestroy)
		{
			NumberItem.RecycleDamageNumber(this);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	public static NumberItem CreateDamageNumber(Vector3 worldPosition, AttackResult result)
	{
		NumberItem numberItem = Create(worldPosition);
		numberItem.Show(result);
		return numberItem;
	}
	public static NumberItem CreateDamageNumber(Vector3 worldPosition, string text)
	{
		NumberItem numberItem = Create(worldPosition);
		numberItem.Show(text);
		return numberItem;
    }
	private static NumberItem Create(Vector3 worldPosition)
	{
		NumberItem numberItem = ResourceManager.Instance.LoadAsset<NumberItem>(ObjectType.GameObject, "UI/Items/NumberItem");
		numberItem.CachedTransform.parent = RootTransform.Instance.UIIconRoot;
		numberItem.CachedTransform.localScale = Vector3.one;
		numberItem.GetComponent<UIWidget>().depth = 20;
		numberItem.UpdatePosition(worldPosition + Vector3.up * 2.5f);
		return numberItem;
    }
	
	public static void RecycleDamageNumber(NumberItem number)
	{
		if (number != null)
		{
			ResourceManager.Instance.RecycleAsset(number.gameObject);
		}
		else
		{
			BaseLogger.Log("Recyle a null HPBar!");
		}
	}
}

