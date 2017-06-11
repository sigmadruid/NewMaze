using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
    public class Adam : Entity, GamePlot.IActor
    {
        public Action CallbackDie;

        public new AdamScript Script
        {
            get { return script as AdamScript; }
            protected set { script = value; }
        }

        public new HeroData Data
        {
            get { return data as HeroData; }
            protected set { data = value; }
        }
        public new HeroInfo Info
        {
            get { return info as HeroInfo; }
            protected set { info = value; }
        }

        public WeaponData LeftWeaponData { get; protected set; }
        public WeaponData RightWeaponData { get; protected set; }

        public bool IsUpdating = true;
        public bool IsSlowUpdating = true;

        public int SkillIndex { get; private set; }
        public Monster TargetMonster { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        private InputManager inputManager;
        private BattleProxy battleProxy;
        private AdamProxy adamProxy;

        private static Adam instance;
        public static Adam Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Update(float deltaTime)
        {
            if(!IsUpdating || Game.Instance.PlotRunner.IsPlaying)
                return;

            if(Script.UseMouse)
            {
                MouseControl();
            }
            else
            {
                AxisControl();
            }


            Info.UpdateBuff(deltaTime);
            Info.UpdateSkill(deltaTime);
        }
        protected override void SlowUpdate()
        {
            if (!IsSlowUpdating)
                return;
        }

        #region Interfaces

        public void Convert(HeroInfo newInfo)
        {
            Data = newInfo.Data;
            Info = newInfo;

            int hash = Animator.StringToHash(newInfo.Data.Trigger);
            Switch(hash);
        }

        public void ClearTarget()
        {
            TargetMonster = null;
            TargetPosition = Vector3.zero;
        }

        #endregion

        #region States

        public bool IsVisible
        {
            get
            {
                return adamProxy.IsVisible;
            }
            set
            {
                adamProxy.IsVisible = value;
                Script.SetTransparent(!adamProxy.IsVisible);
            }
        }

        public bool CanBeAttacked
        {
            get
            {
                return Info.IsAlive && IsVisible;
            }
        }

        #endregion

        #region Control

        private void MouseControl()
        {
            if(inputManager.MouseHitPosition != Vector3.zero)
            {
                //Select target
                TargetPosition = inputManager.MouseHitPosition;
                if(inputManager.CheckMouseHitLayer(Layers.LayerMonster))
                {
                    MonsterScript monsterScript = inputManager.MouseHitObject.GetComponent<MonsterScript>();
                    Monster target = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().GetMonster(monsterScript.Uid);

                    if(target.Info.IsAlive)
                    {
                        TargetMonster = target;
                    }
                    else
                    {
                        TargetMonster = null;
                    }
                }
                else
                {
                    TargetMonster = null;
                }

                //Select skill
                if(inputManager.HitType == MouseHitType.Left)
                    SkillIndex = 1;
                else if(inputManager.HitType == MouseHitType.Right)
                    SkillIndex = 2;
                else
                    SkillIndex = 0;
            }

            if(TargetMonster != null)
            {
                //Choose the skill
                Skill skill = Info.GetSkill(SkillIndex);

                //Cast skill
                if(Info.CanCastSkill(SkillIndex))
                {
                    if(MathUtils.XZSqrDistance(WorldPosition, TargetMonster.WorldPosition) < skill.Data.Range * skill.Data.Range)
                    {
                        MoveByDestination(Vector3.zero);
                        Skill(SkillIndex);
                    }
                    else
                    {
                        if(Info.CanMove())
                        {
                            MoveByDestination(TargetPosition);
                        }
                        else
                        {
                            MoveByDestination(Vector3.zero);
                        }
                    }
                }
            }
            else if(TargetPosition != Vector3.zero)
            {
                if(Info.CanMove() && MathUtils.XZSqrDistance(WorldPosition, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
                {
                    MoveByDestination(TargetPosition);
                }
                else
                {
                    MoveByDestination(Vector3.zero);
                }
            }
            else
            {
                MoveByDestination(Vector3.zero);
            }
        }

        private void AxisControl()
        {
            if(inputManager.MouseHitPosition != Vector3.zero)
            {
                TargetPosition = inputManager.MouseHitPosition;
                Idle();
                LookAt(TargetPosition);

                if(Game.Instance.CurrentStageType == StageEnum.HomeTown)
                    return;

                if(inputManager.HitType == MouseHitType.Left)
                    SkillIndex = 1;
                else if(inputManager.HitType == MouseHitType.Right)
                    SkillIndex = 2;
                else
                    SkillIndex = 0;
                Skill skill = Info.GetSkill(SkillIndex);
                if(Info.CanCastSkill(SkillIndex))
                {
                    Skill(SkillIndex);
                }
            }
            else if(inputManager.DirectionVector != Vector3.zero)
            {
                if(Info.CanMove())
                {
                    MoveByDirection(inputManager.DirectionVector);
                }
            }
            else
            {
                Idle();
            }
        }

        #endregion

        #region Animations

        public void Idle()
        {
            Script.Idle();
        }
        public void MoveByDestination(Vector3 destination)
        {
            Script.MoveByDestination(destination, Info.GetAttribute(BattleAttribute.MoveSpeed));
        }
        public void MoveByDirection(Vector3 direction)
        {
            Script.MoveByDirection(direction, Info.GetAttribute(BattleAttribute.MoveSpeed));
        }
        public void LookAt(Vector3 destPos)
        {
            Script.LookAt(destPos - WorldPosition);
        }
        public void Skill(int skillIndex)
        {
            Info.CurrentSkill = Info.GetSkill(skillIndex);
//            if(Info.CurrentSkill.Data.NeedTarget)
            {
//                if(TargetMonster != null)
                {
//                    Vector3 direction = MathUtils.XZDirection(WorldPosition, TargetMonster.WorldPosition);
//                    SetRotation(direction);
                    Info.CurrentSkill.Cast(Info);
                    Script.Skill(skillIndex, Info.GetAttribute(BattleAttribute.AttackSpeed));
//                    inputManager.PreventMouseAction();
                }
            }

        }
        public void Hit()
        {
            Script.Hit();
        }
        public void Die()
        {
            Script.Die();
        }
        public void PlayAnimation(string trigger)
        {
            int hash = Animator.StringToHash(trigger);
            Script.PlayAnimation(hash);
        }
        public void Switch(int eliteHash)
        {
            Script.Switch(eliteHash);
        }

        #endregion

        #region Event Handlers

        private void OnSkillMiddle(int index)
        {
            if(Info.CurrentSkill != null)
            {
                SkillEffect skillEffect = Info.CurrentSkill.GetEffect(index);
                skillEffect.Cast();
                battleProxy.AttackMonster(skillEffect);
            }
            else
            {
                Debug.LogError("null");
            }
        }
        private void OnSkillEnd()
        {
            ClearTarget();
            Info.CurrentSkill = null;
        }
        private void OnUnsheath()
        {
            if (Data.LeftWeapon != 0)
            {
                LeftWeaponData = WeaponDataManager.Instance.GetData(Data.LeftWeapon) as WeaponData;
                Script.LeftWeapon = ResourceManager.Instance.LoadAsset<WeaponScript>(ObjectType.GameObject, LeftWeaponData.GetResPath());
                Script.LeftWeapon.Attach(Script.LeftHandPosTransform);
            }
            if (Data.RightWeapon != 0)
            {
                RightWeaponData = WeaponDataManager.Instance.GetData(Data.RightWeapon) as WeaponData;
                Script.RightWeapon = ResourceManager.Instance.LoadAsset<WeaponScript>(ObjectType.GameObject, RightWeaponData.GetResPath());
                Script.RightWeapon.Attach(Script.RightHandPosTransform);
            }
        }
        private void OnHitStart()
        {
            Info.IsStunned = true;
        }
        private void OnHitEnd()
        {
            Info.IsStunned = false;
        }
        private void OnDieEnd()
        {
            CallbackDie();
        }
        private void OnTrapAttack(int trapKid)
        {
            TrapData data = TrapDataManager.Instance.GetData(trapKid) as TrapData;
            SkillEffect context = new SkillEffect();
            context.CasterSide = Side.Neutral;
            context.Attack = data.Attack;
            context.Critical = 0;
            battleProxy.DoAttackHero(context);
        }

        #endregion

        public static Adam Create(HeroInfo info)
        {
            Adam adam = new Adam();

            adam.Uid = Guid.NewGuid().ToString();
            adam.Data = info.Data;
            adam.Info = info;
            adam.Script = ResourceManager.Instance.LoadAsset<AdamScript>(ObjectType.GameObject, adam.Data.GetResPath());
            adam.Script.Uid = adam.Uid;
            adam.Script.CallbackUpdate = adam.Update;
            adam.Script.CallbackSlowUpdate = adam.SlowUpdate;
            adam.Script.CallbackHitStart = adam.OnHitStart;
            adam.Script.CallbackHitEnd = adam.OnHitEnd;
            adam.Script.CallbackDieEnd = adam.OnDieEnd;
            adam.Script.CallbackSkillMiddle = adam.OnSkillMiddle;
            adam.Script.CallbackSkillEnd = adam.OnSkillEnd;
            adam.Script.CallbackUnsheath = adam.OnUnsheath;
            adam.Script.CallbackTrapAttack = adam.OnTrapAttack;
            adam.Script.Switch(Animator.StringToHash(adam.Data.Trigger), true);
            adam.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
            adam.adamProxy = ApplicationFacade.Instance.RetrieveProxy<AdamProxy>();
            adam.inputManager = InputManager.Instance;

            instance = adam;

            return adam;
        }
        public static void Recycle()
        {
            Adam hero = instance;
            if (hero != null)
            {
                hero.Data = null;
                hero.IsUpdating = false;
                hero.IsSlowUpdating = false;
                hero.Script.StopAllCoroutines();
                ResourceManager.Instance.RecycleAsset(hero.Script.gameObject);
                hero.Script = null;
                hero.battleProxy = null;
                hero.adamProxy = null;
                instance = null;
            }
            else
            {
                BaseLogger.Log("Recyle a null hero!");
            }
        }

        public new AdamRecord ToRecord()
        {
            AdamRecord record = new AdamRecord();
            record.WorldPosition = new Vector3Record(WorldPosition);
            record.WorldAngle = WorldAngle;
            return record;
        }
    }
    
}