using UnityEngine;
using System;
using System.Collections;

using Base;
using GameLogic;

using HighlightingSystem;

public class MonsterScript : CharacterScript 
{
    public Action<int> CallbackTrapAttack;

	public Transform EmitTransform;

	protected BarItem hpBar;
    protected Highlighter highlighter;
	
	protected override void Awake () 
	{
		base.Awake();
        highlighter = GetComponentInChildren<Highlighter>();
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

    protected override void Update()
    {
        base.Update();

        if(InputManager.Instance.MouseHoverObject == gameObject)
        {
            highlighter.ConstantOnImmediate(Color.white);
        }
        else
        {
            highlighter.ConstantOffImmediate();
        }
        highlighter.ReinitMaterials();
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

