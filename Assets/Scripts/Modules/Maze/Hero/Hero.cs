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
            if (!IsUpdating)
            {
                return;
            }
            Move(inputManager.DirectionVector);
        }
        protected override void SlowUpdate()
        {
            if (!IsSlowUpdating)
            {
                return;
            }

            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, WorldPosition);
        }

        #region States

		public Vector2 MazePosition
		{
			get
			{
				float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
				int col, row;
				MazeUtil.GetMazePosition(WorldPosition, blockSize, out col, out row);
				return new Vector2(col, row);
			}
		}

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
			Script.Attack(OnAttack);
		}
		private void OnAttack(object param)
		{
			Dictionary<AnimatorParamKey, int> paramDic = param as Dictionary<AnimatorParamKey, int>;
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
	}
}

