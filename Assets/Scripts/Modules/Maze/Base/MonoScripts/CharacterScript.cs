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
        public Action<float> CallbackUpdate;
        public Action CallbackSlowUpdate;
        public Action<int> CallbackSkillMiddle;
        public Action CallbackSkillEnd;
        public Action CallbackHitStart;
        public Action CallbackHitEnd;
        public Action CallbackDieEnd;
        public Action CallbackRollStart;
        public Action CallbackRollEnd;

    	public Transform TopPosTransform;
    	public Transform BottomPosTransform;
        public Transform EmitTransform;

        public bool UseMouse = false;
    	
        protected MovementScript movementScript;
    	protected Animator animator;

        private readonly WaitForSeconds SLOW_UPDATE_DELAY = new WaitForSeconds(1f);

        #region Anchor Position
    	
        public Vector3 TopPosition
        {
            get { return TopPosTransform.localPosition; }
        }
        public Vector3 BottomPosition
        {
            get { return BottomPosTransform.localPosition; }
        }
        public Vector3 CenterPosition
        {
            get { return (TopPosTransform.localPosition + BottomPosTransform.localPosition) * 0.5f; }
        }
        public Vector3 EmitPosition
        {
            get { return EmitTransform.localPosition; }
        }

        #endregion

        #region Life Cycle

    	protected virtual void Awake()
    	{
            movementScript = GetComponent<MovementScript>();
    		animator = GetComponent<Animator>();
    	}
    	protected virtual void OnEnable()
    	{
    		GetComponent<Collider>().enabled = true;
    		StartCoroutine(SlowUpdate());
    	}
    	protected virtual void Start ()
    	{
    	}
    	protected virtual void OnDisable()
    	{
    		StopAllCoroutines();
    	}
    	protected virtual void Update () 
    	{
    		if (Game.Instance.IsPause) { return; }

    		if (CallbackUpdate != null)
    		{
                CallbackUpdate(Time.deltaTime);
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
        protected virtual void LateUpdate ()
        {
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
        }

        #endregion

        public override void Pause(bool isPause)
        {
            base.Pause(isPause);
            movementScript.IsControllable = !isPause;
        }

    	#region Behavior

        public virtual void SimpleMove(Vector3 velocity)
        {
            movementScript.SetDestination(Vector3.zero, 0f);
            transform.position += velocity;
        }
        public virtual void MoveByDestination(Vector3 destination, float speed)
    	{
    		if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(destination, speed);
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
    	}
        public virtual void MoveByDirection(Vector3 direction, float speed)
        {
            if (Game.Instance.IsPause) { return; }

            movementScript.SetMove(direction, speed);
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
        }
        public virtual void LookAt(Vector3 direction)
    	{
            movementScript.LookAt(direction);
    	}
        public virtual void Skill(int skillID, float attackSpeed)
    	{
    		if (Game.Instance.IsPause) { return; }

            int skillTrigger = 0;
            if (skillID == 1)
                skillTrigger = AnimatorDataManager.Instance.ParamDoSkill_1;
            else if (skillID == 2)
                skillTrigger = AnimatorDataManager.Instance.ParamDoSkill_2;

            movementScript.SetMove(Vector3.zero, 0);
            movementScript.SetDestination(Vector3.zero, 0);
            animator.speed = attackSpeed;
            animator.SetTrigger(skillTrigger);
    	}
        public virtual void Hit()
    	{
            if (Game.Instance.IsPause) { return; }

            movementScript.SetMove(Vector3.zero, 0);
            movementScript.SetDestination(Vector3.zero, 0f);
            animator.SetTrigger(AnimatorDataManager.Instance.ParamDoHit);
    	}
        public virtual void Roll(Vector3 direction, float speed)
        {
            if(direction == Vector3.zero)
            {
                movementScript.SetRoll(Vector3.zero, 0);
                return;
            }

            direction = direction.normalized;
            movementScript.SetRoll(direction, speed);

            Vector3 forward = MathUtils.XZDirection(transform.forward).normalized;
            float crossY = Vector3.Cross(direction, forward).y;
            float dot = Vector3.Dot(direction, forward);
            if(0.707f < dot && dot <= 1)
            {
                animator.SetInteger(AnimatorDataManager.Instance.ParamRoll, 1);
                Debug.LogError("forward");
            }
            else if(-1 <= dot && dot < -0.707f)
            {
                animator.SetInteger(AnimatorDataManager.Instance.ParamRoll, 2);
                Debug.LogError("backward");
            }
            else
            {
                if(crossY >= 0)
                {
                    animator.SetInteger(AnimatorDataManager.Instance.ParamRoll, 3);
                    Debug.LogError("left");
                }
                else
                {
                    animator.SetInteger(AnimatorDataManager.Instance.ParamRoll, 4);
                    Debug.LogError("right");
                }
            }
        }
        public virtual void Die()
    	{
    		if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(Vector3.zero, 0f);
			GetComponent<Collider>().enabled = false;
			animator.SetTrigger(AnimatorDataManager.Instance.ParamDoDie);
    	}

        public virtual void PlayAnimation(int hash)
        {
            animator.SetTrigger(hash);
        }

    	#endregion

        #region Animation Event Handlers

        public void OnAnimatorStart(AnimatorEventType type)
        {
            switch(type)
            {
                case AnimatorEventType.SKILL_1:
                case AnimatorEventType.SKILL_2:
                    OnSkillStart();
                    break;
                case AnimatorEventType.UNSHEATH:
                    break;
                case AnimatorEventType.SHEATH:
                    break;
                case AnimatorEventType.HIT:
                    OnHitStart();
                    break;
                case AnimatorEventType.DIE:
                    OnDieStart();
                    break;
                case AnimatorEventType.ROLL:
                    OnRollStart();
                    break;
            }
        }
        public void OnAnimatorMiddle(AnimatorEventType type)
        {
            switch(type)
            {
                case AnimatorEventType.SKILL_1:
                case AnimatorEventType.SKILL_2:
                    OnSkillMiddle();
                    break;
                case AnimatorEventType.UNSHEATH:
                    OnUnsheath();
                    break;
                case AnimatorEventType.SHEATH:
                    OnSheath();
                    break;
                case AnimatorEventType.HIT:
                    break;
                case AnimatorEventType.DIE:
                    break;
            }
        }
        public void OnAnimatorEnd(AnimatorEventType type)
        {
            switch(type)
            {
                case AnimatorEventType.SKILL_1:
                case AnimatorEventType.SKILL_2:
                    OnSkillEnd();
                    break;
                case AnimatorEventType.UNSHEATH:
                    break;
                case AnimatorEventType.SHEATH:
                    break;
                case AnimatorEventType.HIT:
                    OnHitEnd();
                    break;
                case AnimatorEventType.DIE:
                    OnDieEnd();
                    break;
                case AnimatorEventType.ROLL:
                    OnRollEnd();
                    break;
            }
        }

        private int effectIndex;
        protected virtual void OnSkillStart()
        {
            effectIndex = 1;
        }
        protected virtual void OnSkillMiddle()
        {
            if (CallbackSkillMiddle != null) CallbackSkillMiddle(effectIndex++);
        }
        protected virtual void OnSkillEnd()
        {
            animator.speed = 1f;
            if (CallbackSkillEnd != null) CallbackSkillEnd();
        }
        protected virtual void OnUnsheath()
        {
        }
        protected virtual void OnSheath()
        {
        }
        protected virtual void OnHitStart()
        {
            if (CallbackHitStart != null) CallbackHitStart();
        }
        protected virtual void OnHitEnd()
        {
            if (CallbackHitEnd != null) CallbackHitEnd();
        }
        protected virtual void OnDieStart()
        {
        }
        protected virtual void OnDieEnd()
        {
            if (CallbackDieEnd != null) CallbackDieEnd();
        }
        protected virtual void OnRollStart()
        {
            if (CallbackRollStart != null) CallbackRollStart();
        }
        protected virtual void OnRollEnd()
        {
            movementScript.SetRoll(Vector3.zero, 0);
            animator.SetInteger(AnimatorDataManager.Instance.ParamRoll, 0);
            if (CallbackRollEnd != null) CallbackRollEnd();
        }

        #endregion

        #region Renderering 

        protected Material material;

        public virtual void SetTransparent(bool isTransparent, float alpha = 0.2f)
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