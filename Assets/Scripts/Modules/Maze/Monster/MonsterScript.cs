using UnityEngine;
using System;
using System.Collections;

using Base;
using GameLogic;

public class MonsterScript : CharacterScript 
{
    public Action<int> CallbackTrapAttack;

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

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Trap))
        {
            TrapScript trap = other.GetComponentInParent<TrapScript>();
            CallbackTrapAttack(trap.Kid);
        }
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
