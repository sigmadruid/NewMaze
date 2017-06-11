using UnityEngine;

using System;

using Base;
using GameLogic;

using HighlightingSystem;

public class NPCScript : TriggerEntityScript
{
    public Transform IconPos;

    private Action callbackEnter;
    private Action callbackExit;

    private HUDIcon hud;
    private Highlighter highlighter;

	void Awake()
	{
        highlighter = GetComponent<Highlighter>();
	}
	void Update()
	{
        if(InputManager.Instance.MouseHoverObject == gameObject)
        {
            highlighter.ConstantOnImmediate(Color.white);
        }
        else
        {
            highlighter.ConstantOffImmediate();
        }
	}
    void LateUpdate()
    {
        if (hud.gameObject.activeSelf)
        {
            hud.UpdatePosition(IconPos.position);
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if (!CheckNPC()) return;

        if (hud != null)
		    hud.gameObject.SetActive(true);
		if (callbackEnter != null)
			callbackEnter();
        AddSelfTrigger();
	}
	void OnTriggerExit(Collider other)
	{
		if (!CheckNPC()) return;

        if (hud != null)
		    hud.gameObject.SetActive(false);
		if (callbackExit != null)
			callbackExit();
        RemoveSelfTrigger();
	}

    public void Init(string uid, Action click, Action enter, Action exit)
    {
        Uid = uid;
        callbackEnter = enter;
        callbackExit = exit;
        if(IconPos != null)
        {
            hud = HUDIcon.Create(HUDIconType.NPC);
            hud.gameObject.SetActive(false);
        }
        transform.parent = RootTransform.Instance.NPCRoot;
    }
    public void Dispose()
    {
        HUDIcon.Recycle(hud);
        hud = null;
        callbackEnter = null;
        callbackExit = null;
        Uid = null;
    }

	private bool CheckNPC()
	{
//		return Game.Instance.CurrentStageType == StageEnum.HomeTown || !Hero.Instance.InBattle;
        return true;
	}
}

