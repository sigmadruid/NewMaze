using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using Battle;
using StaticData;


namespace GameLogic
{
	public class Hero : Entity
	{
		public Utils.CallbackVoid CallbackDie;

		public new HeroData Data
		{
			get { return data as HeroData; }
			protected set { data = value; }
		}
		
		public new HeroScript Script
		{
			get { return script as HeroScript; }
			protected set { script = value; }
		}

		public HeroInfo Info { get; protected set; }

        public bool IsUpdating = true;
        public bool IsSlowUpdating = true;

        public bool CanMove = true;
        public bool CanAttack = true;

		private InputManager inputManager;
		private BattleProxy battleProxy;

		private static Hero instance;
		public static Hero Instance
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

            if(CanAttack && inputManager.MouseHitObject != null)
            {
                if(MathUtils.XZDistance(WorldPosition, inputManager.MouseHitPosition) < GlobalConfig.HeroConfig.AttackDistance)
                {
                    Attack();
                }
                else
                {
                    if(CanMove)
                    {
                        Vector3 direction = MathUtils.XZDirection(WorldPosition, inputManager.MouseHitPosition);
                        Move(direction);
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
                        Vector3 direction = MathUtils.XZDirection(WorldPosition, inputManager.MouseHitPosition);
                        Move(direction);
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
            if(!IsInHall)
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

        public bool IsInHall { get; set; }

        #endregion

        #region Animations

		public void Move(Vector3 direction)
		{
			Script.Move(direction, Info.GetAttribute(BattleAttribute.MoveSpeed));
		}

        public void Attack()
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
                Script.Attack(null, OnAttackEffect, null);
                inputManager.PreventMouseAction();
            }

		}
        private void OnAttackEffect(Dictionary<AnimatorParamKey, string> paramDic)
		{
			if (paramDic != null && paramDic.Count > 0)
			{
				battleProxy.AttackMonster(paramDic);
			}
		}

		public void Function()
		{
		}

		public void Hit()
		{
			Info.LastHitTime = Time.time;
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

        #endregion

		public static Hero Create(int heroKid, HeroInfo info)
		{
			Hero hero = new Hero();

			hero.Data = HeroDataManager.Instance.GetData(heroKid) as HeroData;
			hero.Info = new HeroInfo(hero.Data, info);
			hero.Script = ResourceManager.Instance.LoadAsset<HeroScript>(ObjectType.GameObject, hero.Data.GetResPath());
			hero.Script.CallbackUpdate = hero.Update;
			hero.Script.CallbackSlowUpdate = hero.SlowUpdate;
			hero.Script.CallbackDie = hero.OnDie;
			hero.Script.AnimatorDataDic = AnimatorDataManager.Instance.GetDataDic(hero.Data.Kid);
			hero.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			hero.inputManager = InputManager.Instance;

			instance = hero;

			return hero;
		}
        public static Hero Create(HeroRecord record)
        {
            HeroData data = HeroDataManager.Instance.GetData(record.Kid) as HeroData;
            HeroInfo info = new HeroInfo(data, record);
            return Create(record.Kid, info);
        }
		public static void Recycle()
		{
			Hero hero = instance;
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
            record.IsInHall = IsInHall;
            record.HP = Info.HP;
            return record;
        }
	}
}

