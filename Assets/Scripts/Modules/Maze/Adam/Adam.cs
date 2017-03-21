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

        private GameObject farTarget;

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

        protected override void Update()
        {
            if(!IsUpdating || Game.Instance.PlotRunner.IsPlaying)
                return;

            if((inputManager.CheckMouseHitLayer(Layers.LayerMonster) || farTarget != null) && Info.CanCastSkill(0))
            {
                Skill skill = Info.GetSkill(0);
                GameObject target = inputManager.MouseHitObject;
                if(target == null)
                {
                    MonsterScript monsterScript = farTarget.GetComponent<MonsterScript>();
                    if(monsterScript != null)
                    {
                        Monster monster = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().GetMonster(monsterScript.Uid);
                        if(monster.Info.IsAlive)
                        {
                            target = farTarget;
                        }
                    }
                }
                
                if(Data.AttackType == AttackType.Melee)
                {
                    UpdateMelee(target, skill);
                }
                else
                {
                    UpdateRange(target, skill);
                }
                    
            }
            else
            {
                if(inputManager.MouseHitPosition != Vector3.zero)
                {
                    if(Info.CanMove() && MathUtils.XZSqrDistance(WorldPosition, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
                    {
                        Move(inputManager.MouseHitPosition);
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
            }
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

        #region AI

        private void UpdateMelee(GameObject target, Skill skill)
        {
            if(MathUtils.XZSqrDistance(WorldPosition, target.transform.position) < skill.Data.Range * skill.Data.Range)
            {
                Skill(0);
                farTarget = null;
            }
            else
            {
                if (inputManager.MouseHitObject != null)
                    farTarget = inputManager.MouseHitObject;
                if(Info.CanMove())
                {
                    Move(inputManager.MouseHitPosition);
                }
                else
                {
                    Move(Vector3.zero);
                }
            }
        }
        private void UpdateRange(GameObject target, Skill skill)
        {
            
            if(MathUtils.XZSqrDistance(WorldPosition, target.transform.position) < skill.Data.Range * skill.Data.Range)
            {
                Skill(0);
                farTarget = null;
                //TODO: Try to detect and avoid obstalces
            }
            else
            {
                if (inputManager.MouseHitObject != null)
                    farTarget = inputManager.MouseHitObject;
                if(Info.CanMove())
                {
                    Move(inputManager.MouseHitPosition);
                }
                else
                {
                    Move(Vector3.zero);
                }
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
                GameObject target = farTarget;
                if (target == null)
                    target = inputManager.MouseHitObject;
                if(target != null && target.CompareTag(Tags.Monster))
                {
                    Vector3 direction = MathUtils.XZDirection(WorldPosition, target.transform.position);
                    SetRotation(direction);
                    Info.CurrentSkill.Cast();
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
        }
        public void Switch(int eliteHash)
        {
            Script.Switch(eliteHash);
        }

        #endregion

        #region Event Handlers

        private void OnSkill()
        {
            if(Info.CurrentSkill != null)
                battleProxy.AttackMonster(Info.CurrentSkill);
            else
                Debug.LogError("null");
        }
        private void OnSkillEnd()
        {
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
            AttackContext context = new AttackContext();
            context.CasterSide = Side.Neutral;
            context.Attack = data.Attack;
            context.Critical = 0;
            battleProxy.DoAttackHero(context);
//            Script.Hit(true);
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
            adam.Script.CallbackSkill = adam.OnSkill;
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