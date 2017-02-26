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
    [RequireComponent(typeof(MovementScript))]
    [RequireComponent(typeof(Animator))]
    public class CharacterScript : EntityScript
    {
    	public Transform TopPosTransform;
    	public Transform BottomPosTransform;

        public Action CallbackUpdate;
        public Action CallbackSlowUpdate;
        public Action CallbackDie;
    	
        private Action CallbackAnimatorStarts;
        private Action<Dictionary<AnimatorParamKey, string>> CallbackAnimatorEffect;
        private Action CallbackAnimatorEnds;

        private Action CallbackAttackStarts;
        private Action<Dictionary<AnimatorParamKey, string>> CallbackAttackEffect;
        private Action CallbackAttackEnds;

    	public Dictionary<int, AnimatorData> AnimatorDataDic;

        protected MovementScript movementScript;

    	protected Animator animator;
    	protected bool transitionEnds;
    	protected int currentNameHash;
    	protected AnimatorData currentAnimatorData;

		private readonly WaitForSeconds SLOW_UPDATE_DELAY = new WaitForSeconds(1f);
    	
    	protected virtual void Awake()
    	{
            movementScript = GetComponent<MovementScript>();

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
                if(CallbackAnimatorStarts != null)
                {
                    CallbackAnimatorStarts();
                    CallbackAnimatorStarts = null;
                }
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

    	public void Move(Vector3 destination, float speed)
    	{
    		if (Game.Instance.IsPause) { return; }

            if(CanPlay(AnimatorPriorityEnum.Move))
            {
                movementScript.SetDestination(destination, speed);
                animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
            }
            else
            {
                movementScript.SetDestination(Vector3.zero, 0f);
            }
    	}

    	public void LookAt(Vector3 direction)
    	{
            movementScript.LookAt(direction);
    	}

        public void Attack(Action callbackStarts, Action<Dictionary<AnimatorParamKey, string>> callbackEffect, Action callbackEnds)
    	{
    		if (Game.Instance.IsPause) { return; }

    		if (CanPlay(AnimatorPriorityEnum.Skill))
    		{
    			animator.SetTrigger(AnimatorDataManager.Instance.ParamDoAttack);
    			animator.SetFloat(AnimatorDataManager.Instance.ParamAttackRandomValue, UnityEngine.Random.value);
                transitionEnds = false;
    			
                CallbackAttackStarts = callbackStarts;
                CallbackAttackEffect = callbackEffect;
                CallbackAttackEnds = callbackEnds;

                CallbackAnimatorStarts = OnAttackStarts;
                CallbackAnimatorEffect = OnAttackEffect;
                CallbackAnimatorEnds = OnAttackEnds;

    			OnAttackStarts();
    		}
    	}
    	protected virtual void OnAttackStarts()
    	{
            if (CallbackAttackStarts != null)
                CallbackAttackStarts();
    	}
        protected virtual void OnAttackEffect(Dictionary<AnimatorParamKey, string> paramDic)
        {
            if (CallbackAttackEffect != null)
                CallbackAttackEffect(paramDic);
        }
    	protected virtual void OnAttackEnds()
    	{
            if (CallbackAttackEnds != null)
                CallbackAttackEnds();
    	}

        public void Hit(bool forceStunned = false)
    	{
    		if (Game.Instance.IsPause) { return; }

    		if (CanPlay(AnimatorPriorityEnum.Skill))
    		{
                if(forceStunned || JudgeHit())
                {
                    animator.SetTrigger(AnimatorDataManager.Instance.ParamDoHit);
                    transitionEnds = false;
                }
    		}
    	}

    	public void Die()
    	{
    		if (Game.Instance.IsPause) { return; }

    		if (CanPlay(AnimatorPriorityEnum.Die))
    		{
                movementScript.SetDestination(Vector3.zero, 0f);
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
                AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                Debug.LogError(currentStateInfo.normalizedTime);
    			CallbackDie();
    		}
    	}

    	public override void Pause(bool isPause)
    	{
    		movementScript.IsControllable = !isPause;
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
                material.shader = Utils.TransparentShader;
//                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.SetInt("_Mode", 3);
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                material.SetInt("_ZWrite", 0);
//                material.SetColor("_Color", new Color(1f, 1f, 1f, alpha));
            }
            else
            {
                material.shader = Utils.DiffuseShader;
//                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.SetInt("_Mode", 0);
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//                material.SetInt("_ZWrite", 1);
//                material.SetFloat("_Mode", 1);
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