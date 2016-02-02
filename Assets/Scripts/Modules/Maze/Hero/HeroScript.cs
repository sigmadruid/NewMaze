using UnityEngine;
using System.Collections;

using Base;

public class HeroScript : CharacterScript 
{
	public MeleeWeaponTrail MeleeTrail;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void OnEnable () 
	{
		base.OnEnable();

		Camera mainCamera = Camera.allCameras[0];
		CameraScript cameraController = mainCamera.GetComponent<CameraScript>();
		if (cameraController == null)
		{
			cameraController = mainCamera.gameObject.AddComponent<CameraScript>();
		}
		cameraController.playerTransofrm = CachedTransform;

		MeleeTrail.Emit = false;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override void OnAttackStarts ()
	{
		MeleeTrail.Emit = true;
	}
	protected override void OnAttackEnds ()
	{
		MeleeTrail.Emit = false;
	}


}
