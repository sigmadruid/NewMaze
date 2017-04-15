﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class AdamScript : EntityScript
    {
        public Transform TopPosTransform;
        public Transform EmitTransform;
        public Transform BottomPosTransform;
        public Transform LeftHandPosTransform;
        public Transform RightHandPosTransform;

        public MeleeWeaponTrail MeleeTrail;
        public WeaponScript LeftWeapon;
        public WeaponScript RightWeapon;

        public Action<float> CallbackUpdate;
        public Action CallbackSlowUpdate;
        public Action<int> CallbackSkillMiddle;
        public Action CallbackSkillEnd;
        public Action CallbackDie;
        public Action CallbackUnsheath;
        public Action<int> CallbackTrapAttack;

        protected int currentNameHash;
        protected AnimatorData currentAnimatorData;

        protected MovementScript movementScript;
        protected EffectScript effectScript;
        private Animator animator;

        private readonly WaitForSeconds SLOW_UPDATE_DELAY = new WaitForSeconds(1f);

        #region Life Cycle

        void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
            movementScript = GetComponent<MovementScript>();
            effectScript = GetComponent<EffectScript>();
        }

        protected virtual void Update ()
        {
            if(Game.Instance.IsPause)
            {
                return;
            }

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

        void OnEnable () 
        {
            StartCoroutine(SlowUpdate());

            Camera mainCamera = Camera.allCameras[0];
            Camera3DScript cameraController = mainCamera.GetComponent<Camera3DScript>();
            if (cameraController == null)
            {
                cameraController = mainCamera.gameObject.AddComponent<Camera3DScript>();
            }
            cameraController.playerTransofrm = BottomPosTransform;

//            MeleeTrail.Emit = false;
        }

        void OnDisable()
        {
            StopCoroutine(SlowUpdate());
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Trap))
            {
                TrapScript trap = other.GetComponentInParent<TrapScript>();
                CallbackTrapAttack(trap.Kid);
            }
        }

        #endregion

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

        public override void Pause(bool isPause)
        {
            movementScript.IsControllable = !isPause;
        }

        #region Animation States

        public void Move(Vector3 destination, float speed)
        {
            if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(destination, speed);
            if(destination != Vector3.zero)
                animator.speed = speed / 3f;
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
        }
        public void Skill(int skillID, float attackSpeed)
        {
            int skillTrigger = 0;
            if (skillID == 1)
                skillTrigger = AnimatorDataManager.Instance.ParamDoSkill_1;
            else if (skillID == 2)
                skillTrigger = AnimatorDataManager.Instance.ParamDoSkill_2;

            movementScript.SetDestination(Vector3.zero, 0);
            animator.speed = attackSpeed;
            animator.SetTrigger(skillTrigger);
        }
        public void Hit(bool forceStunned = false)
        {
            if (Game.Instance.IsPause) { return; }

            if(forceStunned || JudgeHit())
            {
                movementScript.SetDestination(Vector3.zero, 0f);
                animator.speed = 1f;
                animator.SetTrigger(AnimatorDataManager.Instance.ParamDoHit);
            }
        }
        public void Die()
        {
            if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(Vector3.zero, 0f);
            GetComponent<Collider>().enabled = false;

            animator.speed = 1f;
            animator.SetTrigger(AnimatorDataManager.Instance.ParamDoDie);
        }
        public void Switch(int eliteHash, bool first = false)
        {
            animator.speed = 1f;
            if (!first)
                animator.SetTrigger(AnimatorDataManager.Instance.ParamDoExit);
            animator.SetTrigger(eliteHash);
        }
        protected bool JudgeHit()
        {
            return false;
//            return RandomUtils.Value() > 0.8f;
        }
        public void PlayAnimation(int hash)
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

        private void OnDieEnd()
        {
            if (CallbackDie != null)
            {
                AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                CallbackDie();
            }
        }
        public void OnUnsheath()
        {
            if (CallbackUnsheath != null)
                CallbackUnsheath();
        }
        public void OnSheath()
        {
            if (LeftWeapon != null)
                ResourceManager.Instance.RecycleAsset(LeftWeapon.gameObject);
            if (RightWeapon != null)
                ResourceManager.Instance.RecycleAsset(RightWeapon.gameObject);
        }
        private int effectIndex;
        public void OnSkillStart()
        {
            effectIndex = 1;
            if (RightWeapon != null)
                RightWeapon.TrailEnabled = true;
        }
        public void OnSkillMiddle()
        {
            if (CallbackSkillMiddle != null)
                CallbackSkillMiddle(effectIndex++);
            Camera3DScript.Instance.Vibrate();
        }
        public void OnSkillEnd()
        {
            if (RightWeapon != null)
                RightWeapon.TrailEnabled = false;
            animator.speed = 1f;
            if (CallbackSkillEnd != null)
                CallbackSkillEnd();
        }

        #endregion

        #region Effect

        public void SetTransparent(bool isTransparent)
        {
            effectScript.SetTransparent(isTransparent);
        }

        #endregion
    }
}

