using UnityEngine;
using System.Collections;

using Base;
using GameLogic;

public enum BulletState
{
	Before,
	Normal,
	After,
}

public class BulletScript : EntityScript 
{
    public System.Action CallbackUpdate;
    public System.Action<Collider> CallbackHit;
    public System.Action CallbackDestroy;

	public GameObject BeforeEffect;
	public GameObject NormalEffect;
	public GameObject AfterEffect;

	private ParticleEmitter[] emitters;

	private BulletState currentState;

	void Awake()
	{
		emitters = GetComponentsInChildren<ParticleEmitter>(true);
	}
	
	void Update () 
	{
		if (Game.Instance.IsPause) return;

		if (CallbackUpdate != null)
		{
			CallbackUpdate();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		CallbackHit(other);
	}

	public void SetState(BulletState state)
	{
		if (currentState == state)
		{
			return;
		}

		if (BeforeEffect != null)
			BeforeEffect.SetActive(false);
		if (NormalEffect != null)
			NormalEffect.SetActive(false);
		if (AfterEffect != null)
			AfterEffect.SetActive(false);

		switch(state)
		{
			case BulletState.Before:
			{
				if (BeforeEffect != null)
					BeforeEffect.SetActive(true);
				break;
			}
			case BulletState.Normal:
			{
				if (NormalEffect != null)
					NormalEffect.SetActive(true);
				break;
			}
			case BulletState.After:
			{
				if (AfterEffect != null)
					AfterEffect.SetActive(true);
				StartCoroutine(DelayDestroy());
				break;
			}
		}
		currentState = state;
	}

	public override void Pause(bool isPause)
	{
		for (int i = 0; i < emitters.Length; ++i)
		{
			ParticleEmitter emitter = emitters[i];
			emitter.enabled = !isPause;
		}
	}

	private IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(2f);

		if (CallbackDestroy != null)
		{
			CallbackDestroy();
		}
	}
}
