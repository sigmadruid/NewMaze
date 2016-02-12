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
	public NPCIcon NpcIcon;

	void Awake()
	{
		NpcIcon = NPCIcon.CreateNPCIcon();
		NpcIcon.CallbackClick = OnIconClick;
		NpcIcon.gameObject.SetActive(false);
	}

	void Update()
	{
		if (NpcIcon.gameObject.activeSelf)
		{
			NpcIcon.UpdatePosition(IconPos.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!CheckNPC()) return;

		NpcIcon.gameObject.SetActive(true);
		if (CallbackEnter != null)
		{
			CallbackEnter();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (!CheckNPC()) return;

		NpcIcon.gameObject.SetActive(false);
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

