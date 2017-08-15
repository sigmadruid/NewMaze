using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
    public class Monster : Entity, GamePlot.IActor
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
			Script.MoveByDestination(Vector3.zero, 0f);
		}
        public void MoveByDestination(Vector3 destination)
		{
			Script.MoveByDestination(destination, Info.GetAttribute(BattleAttribute.MoveSpeed));
		}
		public void LookAt(Vector3 destPos)
		{
            Script.LookAt(destPos - WorldPosition);
		}
        public void Skill(int skillIndex)
		{
            Info.CurrentSkill = Info.GetSkill(skillIndex);
            Info.CurrentSkill.Cast(Info);
            Script.Skill(skillIndex, Info.GetAttribute(BattleAttribute.AttackSpeed));
		}
		public void Hit()
		{
			Script.Hit();
            Script.UpdateHPBar(Info.HP, (int)Info.GetAttribute(BattleAttribute.HP), true);
		}
		public void Die()
		{
			Script.Die();
            SetAtNight(false);

            Tupple<int, Vector3> tupple = new Tupple<int, Vector3>();
            tupple.Item1 = Data.DropKid;
            tupple.Item2 = WorldPosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DROP_CREATED, tupple);
		}
        public void PlayAnimation(string trigger)
        {
            int hash = Animator.StringToHash(trigger);
            Script.PlayAnimation(hash);
        }

        #endregion

        #region Event Handlers

        private void OnSkillMiddle(int index)
        {
            if(Info.CurrentSkill != null)
            {
                SkillEffect skillEffect = Info.CurrentSkill.GetEffect(index);
                skillEffect.Cast();
                battleProxy.AttackHero(this, skillEffect);
            }
            else
            {
                Debug.LogError("null");
            }
        }
        private void OnSkillEnd()
        {
            Info.CurrentSkill = null;
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
            ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().RemoveMonster(Uid);
        }

        private void OnTrapAttack(int trapKid)
        {
            TrapData data = TrapDataManager.Instance.GetData(trapKid) as TrapData;
            SkillEffect context = new SkillEffect();
            context.CasterSide = Side.Neutral;
            context.Attack = data.Attack;
            context.Critical = 0;
            battleProxy.DoAttackMonster(this, context);
            Script.Hit();
        }

		#endregion

        protected override void Update(float deltaTime)
		{
			if (!Info.IsAlive)
				return;
            Info.UpdateBuff(deltaTime);
            Info.UpdateSkill(deltaTime);
            Info.UpdateSP(deltaTime);
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
            if(isNight && Info.IsAlive)
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
            record.IsInHall = Hall.IsActive;
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
        private static void Init(Monster monster)
        {
            monster.Script = ResourceManager.Instance.LoadAsset<MonsterScript>(ObjectType.GameObject, monster.Data.GetResPath());
            monster.Script.Uid = monster.Uid;
            monster.Script.LifeBar = BarItem.Create(monster.Data.Size);
            monster.Script.UpdateHPBar(monster.Info.HP, (int)monster.Info.GetAttribute(BattleAttribute.HP), false);
            monster.Script.transform.parent = RootTransform.Instance.MonsterRoot; 
            monster.Script.CallbackUpdate = monster.Update;
            monster.Script.CallbackSlowUpdate = monster.SlowUpdate;
            monster.Script.CallbackSkillMiddle = monster.OnSkillMiddle;
            monster.Script.CallbackSkillEnd = monster.OnSkillEnd;
            monster.Script.CallbackHitStart = monster.OnHitStart;
            monster.Script.CallbackHitEnd = monster.OnHitEnd;
            monster.Script.CallbackDieEnd = monster.OnDieEnd;
            monster.Script.CallbackTrapAttack = monster.OnTrapAttack;
            monster.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
            ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().AddMonster(monster);
        }

		public static void Recycle(Monster monster)
		{
			if (monster != null)
			{
				monster.Data = null;
                if (monster.Script.LifeBar != null)
                    BarItem.Recycle(monster.Script.LifeBar);
				monster.Script.StopAllCoroutines();
				ResourceManager.Instance.RecycleAsset(monster.Script.gameObject);
				monster.Script = null;
				monster.battleProxy = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null monster!");
			}
		}

		#endregion
		
	}
}

