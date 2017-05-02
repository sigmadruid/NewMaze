using UnityEngine;
using System;
using System.Collections;

using Base;
using GameLogic;

using HighlightingSystem;

public class MonsterScript : CharacterScript 
{
    public Action<int> CallbackTrapAttack;

    public BarItem LifeBar;

    protected Highlighter highlighter;
	
	protected override void Awake () 
	{
		base.Awake();
        highlighter = GetComponentInChildren<Highlighter>();
	}

	protected override void OnEnable () 
	{
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

    protected override void Update()
    {
        base.Update();

        if(InputManager.Instance.MouseHoverObject == gameObject)
        {
            highlighter.ReinitMaterials();
            highlighter.ConstantOnImmediate(Color.white);
        }
        else
        {
            highlighter.ConstantOffImmediate();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (LifeBar != null)
            LifeBar.UpdatePosition(TopPosition);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.CompareTag(Tags.Trap))
        {
            TrapScript trap = other.GetComponentInParent<TrapScript>();
            CallbackTrapAttack(trap.Kid);
        }
    }

    protected override void OnDieStart ()
	{
        base.OnDieStart ();
		if (LifeBar != null)
		{
			BarItem.Recycle(LifeBar);
			LifeBar = null;
		}
	}

	public void UpdateHPBar(int hp, int maxHP)
	{
		LifeBar.UpdateHP(hp, maxHP);
	}
}

