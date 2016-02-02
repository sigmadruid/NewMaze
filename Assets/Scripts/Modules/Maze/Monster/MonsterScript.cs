using UnityEngine;
using System.Collections;

using Base;

public class MonsterScript : CharacterScript 
{
	public Transform EmitTransform;

	protected BarItem hpBar;
	
	protected override void Awake () 
	{
		base.Awake();
	}

	protected override void OnEnable () 
	{
		base.OnEnable();
		hpBar = BarItem.CreateHPBar();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (hpBar != null)
		{
			BarItem.RecycleHPBar(hpBar);
			hpBar = null;
		}
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnDieStarts ()
	{
		base.OnDieStarts ();
		if (hpBar != null)
		{
			BarItem.RecycleHPBar(hpBar);
			hpBar = null;
		}
	}

	public void UpdateHPBar(int hp, int maxHP)
	{
		hpBar.UpdatePosition(TopPosTransform.position);
		hpBar.UpdateHP(hp, maxHP);
	}
}
