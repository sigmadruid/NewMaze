using UnityEngine;

using System;

using Base;
using GameLogic;

public class NPCScript : EntityScript
{
	public Utils.CallbackVoid CallbackEnter;
	public Utils.CallbackVoid CallbackExit;
	public Utils.CallbackVoid CallbackClick;

	public Transform IconPos;

	[HideInInspector]
	public HUDIcon Icon;

	void Awake()
	{
        Icon = HUDIcon.Create(HUDIconType.NPC);
		Icon.CallbackClick = OnIconClick;
		Icon.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Icon.gameObject.activeSelf)
		{
			Icon.UpdatePosition(IconPos.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!CheckNPC()) return;

		Icon.gameObject.SetActive(true);
		if (CallbackEnter != null)
		{
			CallbackEnter();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (!CheckNPC()) return;

		Icon.gameObject.SetActive(false);
		if (CallbackExit != null)
		{
			CallbackExit();
		}
	}

	private void OnIconClick()
	{
		if (CallbackClick != null)
		{
			CallbackClick();
		}
	}

	private bool CheckNPC()
	{
//		return Game.Instance.CurrentStageType == StageEnum.HomeTown || !Hero.Instance.InBattle;
        return true;
	}
}

