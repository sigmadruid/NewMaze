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
                if(inputManager.MouseLeft == MouseHitType.Left)
                    SkillIndex = 1;
                else if(inputManager.MouseLeft == MouseHitType.Right)
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
                        Move(Vector3.zero);
                        Skill(SkillIndex);
                    }
                    else
                    {
                        if(Info.CanMove())
                        {
                            Move(TargetPosition);
                        }
                        else
                        {
                            Move(Vector3.zero);
                        }
                    }
                }
            }
            else if(TargetPosition != Vector3.zero)
            {
                if(Info.CanMove() && MathUtils.XZSqrDistance(WorldPosition, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
                {
                    Move(TargetPosition);
                }
                else
                {
                    Move(Vector3.zero);
                }
            }
            else
            {
                Move(Vector3.zero);
            }

            Info.UpdateBuff(deltaTime);
            Info.UpdateSkill(deltaTime);
        }
        protected override void SlowUpdate()
        {
            if (!IsSlowUpdating)
                return;
            if(Game.Instance.CurrentStageType == StageEnum.Maze && !Info.IsInHall)
            {
                ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, WorldPosition);
            }
        }

        #region Interfaces

        public void Convert(int heroKid)
        {
            HeroData newData = HeroDataManager.Instance.GetData(heroKid) as HeroData;
            Data = newData;
            HeroInfo newInfo = new HeroInfo(newData, Info);
            Info = newInfo;

            int hash = Animator.StringToHash(newData.Trigger);
            Switch(hash);
        }

        public void ClearTarget()
        {
            TargetMonster = null;
            TargetPosition = Vector3.zero;
        }

        #endregion

        #region States

        public bool InBattle 
        { 
            get 
            { 
                return Time.time - Info.LastHitTime <= GlobalConfig.BattleConfig.OutBattleDelay; 
            }
        }

        private bool isVisible = true;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                Script.SetTransparent(!isVisible);
            }
        }

        public bool CanBeAttacked
        {
            get
            {
                return Info.IsAlive && !Info.IsConverting && IsVisible;
            }
        }

        #endregion

        #region Animations

        public void Idle()
        {
            Script.Move(Vector3.zero, 0f);
        }
        public void Move(Vector3 destination)
        {
            Script.Move(destination, Info.GetAttribute(BattleAttribute.MoveSpeed));
        }
        public void LookAt(Vector3 destPos)
        {
//            Script.LookAt(destPos - WorldPosition);
        }
        public void Skill(int skillIndex)
        {
            Info.CurrentSkill = Info.GetSkill(skillIndex);
            if(Info.CurrentSkill.Data.NeedTarget)
            {
                if(TargetMonster != null)
                {
                    Vector3 direction = MathUtils.XZDirection(WorldPosition, TargetMonster.WorldPosition);
                    SetRotation(direction);
                    Info.CurrentSkill.Cast(Info);
                    Script.Skill(skillIndex, Info.GetAttribute(BattleAttribute.AttackSpeed));
                    inputManager.PreventMouseAction();
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
        private void OnDie()
        {
            CallbackDie();
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

        public static Adam Create(int heroKid, HeroInfo info)
        {
            Adam adam = new Adam();

            adam.Uid = Guid.NewGuid().ToString();
            adam.Data = HeroDataManager.Instance.GetData(heroKid) as HeroData;
            adam.Info = new HeroInfo(adam.Data, info);
            adam.Script = ResourceManager.Instance.LoadAsset<AdamScript>(ObjectType.GameObject, adam.Data.GetResPath());
            adam.Script.Uid = adam.Uid;
            adam.Script.CallbackUpdate = adam.Update;
            adam.Script.CallbackSlowUpdate = adam.SlowUpdate;
            adam.Script.CallbackDie = adam.OnDie;
            adam.Script.CallbackSkillMiddle = adam.OnSkillMiddle;
            adam.Script.CallbackSkillEnd = adam.OnSkillEnd;
            adam.Script.CallbackUnsheath = adam.OnUnsheath;
            adam.Script.CallbackTrapAttack = adam.OnTrapAttack;
            adam.Script.Switch(Animator.StringToHash(adam.Data.Trigger), true);
            adam.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
            adam.inputManager = InputManager.Instance;

            instance = adam;

            return adam;
        }
        public static Adam Create(HeroRecord record)
        {
            HeroData data = HeroDataManager.Instance.GetData(record.Kid) as HeroData;
            HeroInfo info = new HeroInfo(data, record);
            return Create(record.Kid, info);
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
                instance = null;
            }
            else
            {
                BaseLogger.Log("Recyle a null hero!");
            }
        }

        public new HeroRecord ToRecord()
        {
            HeroRecord record = new HeroRecord();
            record.Uid = Uid;
            record.Kid = Data.Kid;
            record.WorldPosition = new Vector3Record(WorldPosition);
            record.WorldAngle = WorldAngle;
            record.IsVisible = IsVisible;
            record.IsInHall = Info.IsInHall;
            record.HP = Info.HP;
            return record;
        }
    }
    
}