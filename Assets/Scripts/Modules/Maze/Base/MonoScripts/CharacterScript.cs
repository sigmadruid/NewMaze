using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

using DG.Tweening;

namespace GameLogic
{
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
    	
        private System.Action CallbackAnimatorTransition;
        private System.Action<Dictionary<AnimatorParamKey, int>> CallbackAnimatorEffect;
        private System.Action CallbackAnimatorEnds;

    	public Dictionary<int, AnimatorData> AnimatorDataDic;

    	protected MoveScript moveScript;

    	protected Animator animator;
    	protected bool transitionEnds;
    	protected int currentNameHash;
    	protected AnimatorData currentAnimatorData;

		private readonly WaitForSeconds SLOW_UPDATE_DELAY = new WaitForSeconds(1f);
    	
    	protected virtual void Awake()
    	{
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
    		if (currentNameHash != currentStateInfo.fullPathHash)
    		{
    			currentNameHash = currentStateInfo.fullPathHash;
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

    	#region Behavior

    	public void Move(Vector3 direction, float velocity)
    	{
    		if (Game.Instance.IsPause) { return; }

    		if (CanPlay(AnimatorPriorityEnum.Run))
    		{
				moveScript.Move(direction, velocity);
    			animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, moveScript.IsMoving);
    		}
    		else
    		{
    			moveScript.Move(Vector3.zero, 0f);
    		}
    	}

    	public void LookAt(Vector3 destPos)
    	{
    		Vector3 lookDirection = destPos - transform.position;
    		moveScript.LookAt(lookDirection);
    	}

        public void Attack(System.Action<Dictionary<AnimatorParamKey, int>> callbackEffect)
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
    			moveScript.Move(Vector3.zero, 0f);
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

        protected bool JudgeHit()
        {
            return RandomUtils.Value() > 0.8f;
        }
        protected bool CanPlay(AnimatorPriorityEnum priority)
        {
            return priority == currentAnimatorData.Priority && currentAnimatorData.IsLoop || (int)priority > (int)currentAnimatorData.Priority;
        }

    	#endregion

        #region Renderering 

        protected Material material;

        public void SetTransparent(bool isTransparent, float alpha = 0.2f)
        {
            InitMaterial();
            if(isTransparent)
            {
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetInt("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.SetColor("_Color", new Color(1f, 1f, 1f, alpha));
            }
            else
            {
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetInt("_Mode", 0);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.SetFloat("_Mode", 1);
            }
        }

        public void SetEmissionColor(Color color, float duration)
        {
            InitMaterial();
            material.EnableKeyword("_EMISSION");
//            material.SetColor("_EmissionColor", color);
            material.DOColor(color, "_EmissionColor", duration);
        }

        private void InitMaterial()
        {
            if(material == null)
            {
                material = GetComponentInChildren<Renderer>().material;
            }
        }

        #endregion

    }

}