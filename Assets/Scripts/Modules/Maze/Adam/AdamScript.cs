using UnityEngine;

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
        public Transform BottomPosTransform;
        public MeleeWeaponTrail MeleeTrail;
        public GameObject sword;
        public GameObject axe;

        public Action CallbackUpdate;
        public Action CallbackSlowUpdate;
        public Action CallbackSkill;
        public Action CallbackDie;
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

        void OnEnable () 
        {
            StartCoroutine(SlowUpdate());

            Camera mainCamera = Camera.allCameras[0];
            Camera3DScript cameraController = mainCamera.GetComponent<Camera3DScript>();
            if (cameraController == null)
            {
                cameraController = mainCamera.gameObject.AddComponent<Camera3DScript>();
            }
            cameraController.playerTransofrm = transform;

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

        public override void Pause(bool isPause)
        {
            movementScript.IsControllable = !isPause;
        }

        #region Animation States

        public void Move(Vector3 destination, float speed)
        {
            if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(destination, speed);
            animator.speed = speed / 3f;
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
        }
        public void Skill(int skillID, float attackSpeed)
        {
            int skillTrigger = AnimatorDataManager.Instance.ParamDoSkill;
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
        public void Switch(int eliteHash)
        {
            animator.speed = 1f;
            animator.SetTrigger(AnimatorDataManager.Instance.ParamDoExit);
            animator.SetTrigger(eliteHash);
        }
        protected bool JudgeHit()
        {
            return false;
//            return RandomUtils.Value() > 0.8f;
        }

        #endregion

        #region Animation Event Handlers

        private void OnDieStart(string state)
        {
        }
        private void OnDieEnd(string state)
        {
            if (CallbackDie != null)
            {
                AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                Debug.LogError(currentStateInfo.normalizedTime);
                CallbackDie();
            }
        }
        public void OnUnsheath(string state)
        {
            int hash = Animator.StringToHash(state);
            if(hash == AnimatorDataManager.Instance.ParamDoAxe)
            {
                axe.SetActive(true);
            }
            else if (hash == AnimatorDataManager.Instance.ParamDoSword)
            {
                sword.SetActive(true);
            }
                
        }
        public void OnSheath(string state)
        {
            int hash = Animator.StringToHash(state);
            if(hash == AnimatorDataManager.Instance.ParamDoAxe)
            {
                axe.SetActive(false);
            }
            else if (hash == AnimatorDataManager.Instance.ParamDoSword)
            {
                sword.SetActive(false);
            }
        }
        public void OnSkillStart(string state)
        {
//            MeleeTrail.Emit = true;
        }
        public void OnSkillMiddle(string state)
        {
            CallbackSkill();
        }
        public void OnSkillEnd(string state)
        {
//            MeleeTrail.Emit = false;
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

