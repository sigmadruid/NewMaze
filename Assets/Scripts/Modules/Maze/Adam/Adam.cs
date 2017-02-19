using UnityEngine;

using System;
using System.Collections;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
    public class Adam : Entity
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

        public bool IsUpdating = true;
        public bool IsSlowUpdating = true;

        public bool CanMove = true;
        public bool CanAttack = true;

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
            if(!IsUpdating)
                return;

            if(CanAttack && inputManager.CheckMouseHitLayer(Layers.LayerMonster))
            {
                if(MathUtils.XZDistance(WorldPosition, inputManager.MouseHitPosition) < GlobalConfig.HeroConfig.AttackDistance)
                {
                    Skill(0);
                }
                else
                {
                    if(CanMove)
                    {
                        Move(inputManager.MouseHitPosition);
                    }
                    else
                    {
                        Move(Vector3.zero);
                    }
                }
            }
            else
            {
                if(inputManager.MouseHitPosition != Vector3.zero)
                {
                    if(CanMove && MathUtils.XZSqrDistance(WorldPosition, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
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
            if(!Info.IsInHall)
            {
                ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, WorldPosition);
            }
        }

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
            Script.Idle();
        }
        public void Move(Vector3 destination)
        {
            Script.Move(destination, Info.GetAttribute(BattleAttribute.MoveSpeed));
        }
        public void Skill(int skillID)
        {
            if (!Info.IsAlive)
            {
                return;
            }

            GameObject target = inputManager.MouseHitObject;
            if(target != null && target.CompareTag(Tags.Monster))
            {
                Vector3 direction = MathUtils.XZDirection(WorldPosition, target.transform.position);
                SetRotation(direction);
                Script.Skill(skillID);
                inputManager.PreventMouseAction();
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
        public void Switch(string state)
        {
            Script.Switch(state);
        }

        #endregion

        #region Event Handlers

        private void OnSkill()
        {
            //TODO: Make a wrapper for the param dic
//            if (paramDic != null && paramDic.Count > 0)
//            {
//                battleProxy.AttackMonster(paramDic);
//            }
        }

        private void OnTrapAttack(int trapKid)
        {
            TrapData data = TrapDataManager.Instance.GetData(trapKid) as TrapData;
            AttackContext context = new AttackContext();
            context.Side = Side.Neutral;
            context.Attack = data.Attack;
            context.Critical = 0;
            battleProxy.DoAttackHero(context);
            Script.Hit(true);
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
            adam.Script.CallbackTrapAttack = adam.OnTrapAttack;
//            hero.Script.AnimatorDataDic = AnimatorDataManager.Instance.GetDataDic(hero.Data.Kid);
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