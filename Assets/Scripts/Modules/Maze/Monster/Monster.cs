using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
	public class Monster : Entity
	{
        public bool InHall;

        public new MonsterScript Script
        {
            get { return script as MonsterScript; }
            protected set { script = value; }
        }
        public new MonsterData Data
        {
            get { return data as MonsterData; }
            protected set { data = value; }
        }
        public new MonsterInfo Info
        {
            get { return info as MonsterInfo; }
            protected set { info = value; }
        }

		private BattleProxy battleProxy;

		#region Animations

		public void Idle()
		{
			Script.Move(Vector3.zero, 0f);
		}
		public void Move(Vector3 direction)
		{
			Script.Move(direction, Info.GetAttribute(BattleAttribute.MoveSpeed));
		}
		public void LookAt(Vector3 destPos)
		{
			Script.LookAt(destPos);
		}
		public void Attack()
		{
			Script.Attack(null, OnAttack, null);
		}
        private void OnAttack(Dictionary<AnimatorParamKey, string> paramDic)
		{
			battleProxy.AttackHero(this, paramDic);
		}
		public void Hit()
		{
			Script.Hit();
		}
		public void Die()
		{
			Script.Die();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DROP_CREATED, this);
		}

        #endregion

        #region Event Handlers

        private void OnTrapAttack(int trapKid)
        {
            TrapData data = TrapDataManager.Instance.GetData(trapKid) as TrapData;
            AttackContext context = new AttackContext();
            context.Side = Side.Neutral;
            context.Attack = data.Attack;
            context.Critical = 0;
            battleProxy.DoAttackMonster(this, context);
            Script.Hit(true);
        }

		#endregion

		protected override void Update()
		{
			if (!Info.IsAlive)
			{
				return;
			}

            Script.UpdateHPBar(Info.HP, (int)Info.GetAttribute(BattleAttribute.HP));
		}

        public void AddBuff(int kid, float remainTime = 0f)
        {
            Buff buff = Buff.Create(kid, remainTime);
            Info.AddBuff(buff);
            buff.Start(Script);
        }

        public void RemoveBuff(int kid)
        {
            Buff buff = Info.GetBuff(kid);
            if(buff != null)
            {
                Info.RemoveBuff(kid);
                buff.End();
            }
        }

        public void SetAtNight(bool isNight)
        {
            int nightFuryBuffID = GlobalConfig.EnvironmentConfig.NightFuryBuffID;
            if(isNight)
            {
                AddBuff(nightFuryBuffID);
            }
            else
            {
                RemoveBuff(nightFuryBuffID);
            }
        }

		public new MonsterRecord ToRecord()
		{
			MonsterRecord record = new MonsterRecord();
			record.Uid = Uid;
			record.Kid = Data.Kid;
            record.WorldPosition =new Vector3Record(WorldPosition);
            record.WorldAngle = WorldAngle;
			record.HP = Info.HP;
            record.buffRemainTimeDic = Info.RecordBuff();
			return record;
		}

		#region Factory Methods

		public static Monster Create(MonsterRecord record)
		{
			Monster monster = new Monster();
			if (record != null)
			{
				monster.Uid = record.Uid;
                monster.Data = MonsterDataManager.Instance.GetData(record.Kid) as MonsterData;
				monster.Info = new MonsterInfo(monster.Data, record);
                Init(monster);
                Dictionary<int, float>.Enumerator enumertor = record.buffRemainTimeDic.GetEnumerator();
                while (enumertor.MoveNext())
                {
                    monster.AddBuff(enumertor.Current.Key, enumertor.Current.Value);
                }
			}
			else
			{
				monster.Uid = Guid.NewGuid().ToString();
				monster.Data = MonsterDataManager.Instance.GetRandomMonsterData();
				monster.Info = new MonsterInfo(monster.Data);
                Init(monster);
			}

			return monster;
		}
        public static Monster Create(int kid)
        {
            Monster monster = new Monster();
            monster.Uid = Guid.NewGuid().ToString();
            monster.Data = MonsterDataManager.Instance.GetData(kid) as MonsterData;
            monster.Info = new MonsterInfo(monster.Data);
            Init(monster);
            return monster;
        }
        public static void Init(Monster monster)
        {
            monster.Script = ResourceManager.Instance.LoadAsset<MonsterScript>(ObjectType.GameObject, monster.Data.GetResPath());
            monster.Script.AnimatorDataDic = AnimatorDataManager.Instance.GetDataDic(monster.Data.Kid);
            monster.Script.transform.parent = RootTransform.Instance.MonsterRoot; 
            monster.Script.CallbackUpdate = monster.Update;
            monster.Script.CallbackSlowUpdate = monster.SlowUpdate;
            monster.Script.CallbackTrapAttack = monster.OnTrapAttack;
            monster.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();

            Game.Instance.AICore.AddAI(monster);
        }

		public static void Recycle(Monster monster)
		{
			if (monster != null)
			{
				monster.Data = null;
				monster.Script.StopAllCoroutines();
				ResourceManager.Instance.RecycleAsset(monster.Script.gameObject);
				monster.Script = null;
				monster.battleProxy = null;

				Game.Instance.AICore.RemoveAI(monster.Uid);
			}
			else
			{
				BaseLogger.Log("Recyle a null monster!");
			}
		}

		#endregion
		
	}
}

