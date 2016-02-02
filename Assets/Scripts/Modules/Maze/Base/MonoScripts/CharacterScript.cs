using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoveScript))]
[RequireComponent(typeof(Animator))]
public class CharacterScript : EntityScript
{
	public Transform TopPosTransform;
	public Transform BottomPosTransform;

	public Utils.CallbackVoid CallbackUpdate;
	public Utils.CallbackVoid CallbackSlowUpdate;
	public Utils.CallbackVoid CallbackDie;
	
	private Utils.CallbackVoid CallbackAnimatorTransition;
	private Utils.CallbackParam CallbackAnimatorEffect;
	private Utils.CallbackVoid CallbackAnimatorEnds;

	public Dictionary<int, AnimatorData> AnimatorDataDic;

	protected MoveScript moveScript;

	protected Animator animator;
	protected bool transitionEnds;
	protected int currentNameHash;
	protected AnimatorData currentAnimatorData;

	private WaitForSeconds SLOW_UPDATE_DELAY;
	
	protected virtual void Awake()
	{
		SLOW_UPDATE_DELAY = new WaitForSeconds(1f);

		moveScript = GetComponent<MoveScript>();

		animator = GetComponent<Animator>();
		currentNameHash = 0;

		GetComponent<Rigidbody>().isKinematic = true;
	}

	protected virtual void OnEnable()
	{
		GetComponent<Collider>().enabled = true;
		StartCoroutine(SlowUpdate());
	}

	protected virtual void Start ()
	{
		currentNameHash = AnimatorDataManager.Instance.IdleHash;
		currentAnimatorData = AnimatorDataDic[currentNameHash];
	}

	protected virtual void OnDisable()
	{
		StopAllCoroutines();
	}
	
	protected virtual void Update () 
	{
		if (Game.Instance.IsPause) { return; }

		AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (currentNameHash != currentStateInfo.nameHash)
		{
			currentNameHash = currentStateInfo.nameHash;
			currentAnimatorData = AnimatorDataDic[currentNameHash];
			transitionEnds = true;
		}

		if (transitionEnds)
		{
			if (CallbackAnimatorEffect != null && currentStateInfo.normalizedTime >= currentAnimatorData.NormalTime)
			{
				CallbackAnimatorEffect(currentAnimatorData.ParamDic);
				CallbackAnimatorEffect = null;
			}
			if (CallbackAnimatorEnds != null && currentStateInfo.normalizedTime >= 0.9f)
			{
				CallbackAnimatorEnds();
				CallbackAnimatorEnds = null;
			}
		}

		if (CallbackUpdate != null)
		{
			CallbackUpdate();
		}
	}
	private IEnumerator SlowUpdate()
	{
		while(true)
		{
			if (Game.Instance.IsPause) 
				yield return SLOW_UPDATE_DELAY;
				
			if (CallbackSlowUpdate != null)
			{
				CallbackSlowUpdate();
			}
			yield return SLOW_UPDATE_DELAY;
		}
	}

	#region Controlling

	public void Move(Vector3 direction)
	{
		if (Game.Instance.IsPause) { return; }

		if (CanPlay(AnimatorPriorityEnum.Run))
		{
			moveScript.Move(direction);
			animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, moveScript.IsMoving);
		}
		else
		{
			moveScript.Move(Vector3.zero);
		}
	}

	public void LookAt(Vector3 destPos)
	{
		Vector3 lookDirection = destPos - CachedTransform.position;
		moveScript.LookAt(lookDirection);
	}

	public void Attack(Utils.CallbackParam callbackEffect)
	{
		if (Game.Instance.IsPause) { return; }

		if (CanPlay(AnimatorPriorityEnum.Attack))
		{
			animator.SetTrigger(AnimatorDataManager.Instance.ParamDoAttack);
			animator.SetFloat(AnimatorDataManager.Instance.ParamAttackRandomValue, UnityEngine.Random.value);
			transitionEnds = false;
			
			CallbackAnimatorEffect = callbackEffect;
			CallbackAnimatorEnds = OnAttackEnds;

			OnAttackStarts();
		}
	}
	protected virtual void OnAttackStarts()
	{
	}
	protected virtual void OnAttackEnds()
	{
	}

	public void Hit()
	{
		if (Game.Instance.IsPause) { return; }

		if (CanPlay(AnimatorPriorityEnum.Attack) && JudgeHit())
		{
			animator.SetTrigger(AnimatorDataManager.Instance.ParamIsHit);
			transitionEnds = false;
		}
	}

	public void Die()
	{
		if (Game.Instance.IsPause) { return; }

		if (CanPlay(AnimatorPriorityEnum.Die))
		{
			moveScript.Move(Vector3.zero);
			GetComponent<Collider>().enabled = false;

			animator.SetTrigger(AnimatorDataManager.Instance.ParamDoDie);
			transitionEnds = false;

			CallbackAnimatorEnds = OnDieEnds;

			OnDieStarts();
		}
	}
	protected virtual void OnDieStarts()
	{
	}
	protected virtual void OnDieEnds()
	{
		if (CallbackDie != null)
		{
			CallbackDie();
		}
	}

	public override void Pause(bool isPause)
	{
		moveScript.IsControllable = !isPause;
	}

	#endregion

	#region Others

	protected bool JudgeHit()
	{
		return UnityEngine.Random.value > 0.8f;
	}
	
	protected bool CanPlay(AnimatorPriorityEnum priority)
	{
		return priority == currentAnimatorData.Priority && currentAnimatorData.IsLoop || (int)priority > (int)currentAnimatorData.Priority;
	}

	#endregion
}

