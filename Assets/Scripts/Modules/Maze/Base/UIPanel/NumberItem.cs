using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using Battle;
using GameLogic;

using DG.Tweening;

public class NumberItem : BaseScreenItem
{
	public bool AutoDestroy = true;
    public float DestroyDelay = 1f;

    private bool startDestroy;
    private float timer;
    private Text labelNumber;
    private DOTweenAnimation tween;

	protected override void Awake()
	{
		base.Awake();

        labelNumber = GetComponent<Text>();
        tween = GetComponent<DOTweenAnimation>();
	}
	
	public void Show(AttackResult result)
	{
		gameObject.SetActive(true);

		labelNumber.color = result.IsCritical ? Color.red : Color.white;
        transform.localScale = Vector3.one * 4;
        tween.DORewind();
        tween.DOPlayForward();

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
	}

    public void OnFinished()
	{
        startDestroy = true;
        timer = 0f;
	}

    void Update()
    {
        if(startDestroy)
        {
            timer += Time.deltaTime;
            if(timer > DestroyDelay)
            {
                startDestroy = false;
                if(AutoDestroy)
                {
                    NumberItem.Recycle(this);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

	public static NumberItem Create(Vector3 worldPosition, AttackResult result)
	{
        NumberItem numberItem = DoCreate(worldPosition);
		numberItem.Show(result);
		return numberItem;
	}
	public static NumberItem Create(Vector3 worldPosition, string text)
	{
        NumberItem numberItem = DoCreate(worldPosition);
		numberItem.Show(text);
		return numberItem;
    }
	private static NumberItem DoCreate(Vector3 worldPosition)
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

