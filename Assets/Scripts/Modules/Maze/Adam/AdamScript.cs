using UnityEngine;

using System;
using System.Collections;

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

            MeleeTrail.Emit = false;
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

        public void Idle()
        {
            animator.SetBool(AdamConstDef.BOOL_IS_MOVING, false);
        }
        public void Move(Vector3 destination, float speed)
        {
            animator.SetBool(AdamConstDef.BOOL_IS_MOVING, true);
            if (Game.Instance.IsPause) { return; }

            if(CanPlay(AnimatorPriorityEnum.Run))
            {
                movementScript.SetDestination(destination, speed);
                animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
            }
            else
            {
                movementScript.SetDestination(Vector3.zero, 0f);
            }
        }
        public void Skill(int skillID)
        {
            string skillTrigger = AdamConstDef.TRIGGER_SKILLS[skillID];
            animator.SetTrigger(skillTrigger);
        }
        public void Hit(bool forceStunned = false)
        {
            if (Game.Instance.IsPause) { return; }

            if (CanPlay(AnimatorPriorityEnum.Attack))
            {
                if(forceStunned || JudgeHit())
                {
                    animator.SetTrigger(AdamConstDef.TRIGGER_HIT);
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

                animator.SetTrigger(AdamConstDef.TRIGGER_DIE);
            }
        }
        public void Switch(string state)
        {
            animator.SetTrigger(AdamConstDef.TRIGGER_EXIT);
            animator.SetTrigger(state);
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
            if(state == AdamConstDef.STATE_AXE)
            {
                axe.SetActive(true);
            }
            else if (state == AdamConstDef.STATE_SWORD)
            {
                sword.SetActive(true);
            }
                
        }
        public void OnSheath(string state)
        {
            if(state == AdamConstDef.STATE_AXE)
            {
                axe.SetActive(false);
            }
            else if (state == AdamConstDef.STATE_SWORD)
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

