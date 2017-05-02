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
        public Transform EmitTransform;

        public Action<float> CallbackUpdate;
        public Action CallbackSlowUpdate;
        public Action<int> CallbackSkillMiddle;
        public Action CallbackSkillEnd;
        public Action CallbackDie;
    	
        protected MovementScript movementScript;
    	protected Animator animator;

        private readonly WaitForSeconds SLOW_UPDATE_DELAY = new WaitForSeconds(1f);

        #region Anchor Position
    	
        public Vector3 TopPosition
        {
            get { return TopPosTransform.position; }
        }
        public Vector3 BottomPosition
        {
            get { return BottomPosTransform.position; }
        }
        public Vector3 CenterPosition
        {
            get { return (TopPosTransform.position + BottomPosTransform.position) * 0.5f; }
        }
        public Vector3 EmitPosition
        {
            get { return EmitTransform.position; }
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

        public virtual void Pause(bool isPause)
        {
            movementScript.IsControllable = !isPause;
        }

    	#region Behavior

        public virtual void SimpleMove(Vector3 velocity)
        {
            movementScript.SetDestination(Vector3.zero, 0f);
            transform.position += velocity;
        }
        public virtual void Move(Vector3 destination, float speed)
    	{
    		if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(destination, speed);
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

            movementScript.SetDestination(Vector3.zero, 0);
            animator.speed = attackSpeed;
            animator.SetTrigger(skillTrigger);
    	}
        public virtual void Hit(bool forceStunned = false)
    	{
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
                case AnimatorEventType.DIE:
                    OnDieStart();
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
                case AnimatorEventType.DIE:
                    OnDieEnd();
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
            if (CallbackSkillMiddle != null)
                CallbackSkillMiddle(effectIndex++);
        }
        protected virtual void OnSkillEnd()
        {
            animator.speed = 1f;
            if (CallbackSkillEnd != null)
                CallbackSkillEnd();
        }
        protected virtual void OnUnsheath()
        {
        }
        protected virtual void OnSheath()
        {
        }
        protected virtual void OnDieStart()
        {
        }
        protected virtual void OnDieEnd()
        {
            if (CallbackDie != null)
            {
                CallbackDie();
            }
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