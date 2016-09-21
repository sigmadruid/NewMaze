using UnityEngine;
using System;
using System.Collections;

using Base;
using GameLogic;

public class HeroScript : CharacterScript 
{
    public Action<int> CallbackTrapAttack;

	public MeleeWeaponTrail MeleeTrail;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void OnEnable () 
	{
		base.OnEnable();

		Camera mainCamera = Camera.allCameras[0];
		Camera3DScript cameraController = mainCamera.GetComponent<Camera3DScript>();
		if (cameraController == null)
		{
			cameraController = mainCamera.gameObject.AddComponent<Camera3DScript>();
		}
		cameraController.playerTransofrm = transform;

		MeleeTrail.Emit = false;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Trap))
        {
            TrapScript trap = other.GetComponentInParent<TrapScript>();
            CallbackTrapAttack(trap.Kid);
        }
    }

	protected override void OnAttackStarts ()
	{
        base.OnAttackStarts();
		MeleeTrail.Emit = true;
	}
	protected override void OnAttackEnds ()
	{
        base.OnAttackEnds();
		MeleeTrail.Emit = false;
	}


}
