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
			CallbackUpdate();
	}

	void OnTriggerEnter(Collider other)
	{
        if (CallbackHit != null)
	        CallbackHit(other);
	}

    public void SetState(BulletState state, float duration = 0)
	{
		if (currentState == state)
		{
			return;
		}
        if(BeforeEffect != null)
        {
            BeforeEffect.SetActive(state == BulletState.Before);
        }
        if(NormalEffect != null)
        {
            NormalEffect.SetActive(state == BulletState.Normal);
        }
        if(AfterEffect != null)
        {
            AfterEffect.SetActive(state == BulletState.After);
        }
        switch(state)
        {
            case BulletState.Before:
            {
                break;
            }
            case BulletState.Normal:
            {
                break;
            }
            case BulletState.After:
            {
                if(duration != 0)
                    StartCoroutine(DelayDestroy(duration));
                else if(CallbackDestroy != null)
                    CallbackDestroy();
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

    private IEnumerator DelayDestroy(float duration)
	{
        yield return new WaitForSeconds(duration);

		if (CallbackDestroy != null)
			CallbackDestroy();
	}
}
