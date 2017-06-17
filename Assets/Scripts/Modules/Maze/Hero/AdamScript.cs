using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class AdamScript : CharacterScript
    {
        public Transform LeftHandPosTransform;
        public Transform RightHandPosTransform;
        public WeaponScript LeftWeapon;
        public WeaponScript RightWeapon;

        public Action CallbackUnsheath;
        public Action<int> CallbackTrapAttack;

        protected EffectScript effectScript;

        private int currentEliteHash;

        #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            animator.applyRootMotion = false;
            effectScript = GetComponent<EffectScript>();
        }

        protected override void Update ()
        {
            base.Update();
            if (Game.Instance.IsPause) { return; }
        }

        protected override void OnEnable () 
        {
            base.OnEnable();

            Camera mainCamera = Camera.allCameras[0];
            Camera3DScript cameraController = mainCamera.GetComponent<Camera3DScript>();
            if (cameraController == null)
            {
                cameraController = mainCamera.gameObject.AddComponent<Camera3DScript>();
            }
            cameraController.playerTransofrm = BottomPosTransform;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if(other.CompareTag(Tags.Trap))
            {
                TrapScript trap = other.GetComponentInParent<TrapScript>();
                CallbackTrapAttack(trap.Kid);
            }
        }

        #endregion

        #region Animation States

        public void Idle()
        {
            movementScript.SetDirection(Vector3.zero, 0);
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, false);
        }

        public override void MoveByDestination(Vector3 destination, float speed)
        {
            if (Game.Instance.IsPause) { return; }

            movementScript.SetDestination(destination, speed);
            if(destination != Vector3.zero)
                animator.speed = speed / 3f;
            animator.SetBool(AnimatorDataManager.Instance.ParamIsMoving, movementScript.IsMoving);
        }
        public override void Skill(int skillID, float attackSpeed)
        {
            base.Skill(skillID, attackSpeed);
        }
        public override void Hit()
        {
            base.Hit();
            animator.speed = 1f;
            animator.SetTrigger(currentEliteHash);
        }
        public override void Die()
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
            currentEliteHash = eliteHash;
            animator.SetTrigger(eliteHash);
        }
        public override void PlayAnimation(int hash)
        {
            animator.SetTrigger(hash);
        }

        #endregion

        #region Animation Event Handlers

        protected override void OnUnsheath()
        {
            base.OnUnsheath();
            if (CallbackUnsheath != null)
                CallbackUnsheath();
        }
        protected override void OnSheath()
        {
            base.OnSheath();
            if (LeftWeapon != null)
                ResourceManager.Instance.RecycleAsset(LeftWeapon.gameObject);
            if (RightWeapon != null)
                ResourceManager.Instance.RecycleAsset(RightWeapon.gameObject);
        }
        protected override void OnSkillStart()
        {
            base.OnSkillStart();
            if (RightWeapon != null)
                RightWeapon.TrailEnabled = true;
        }
        protected override void OnSkillEnd()
        {
            base.OnSkillEnd();
            if (RightWeapon != null)
                RightWeapon.TrailEnabled = false;
        }

        #endregion

        #region Effect

        public override void SetTransparent(bool isTransparent, float alpha = 0.2f)
        {
            effectScript.SetTransparent(isTransparent, alpha);
        }

        #endregion
    }
}

