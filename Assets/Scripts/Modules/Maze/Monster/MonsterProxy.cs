using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
	public class MonsterProxy : Proxy
	{
		//Active monsters.
        private Dictionary<string, Monster> monsterBlockDic = new Dictionary<string, Monster>();
        private Dictionary<string, Monster> monsterHallDic = new Dictionary<string, Monster>();
		//Inactive monsters. Record them.
        public Dictionary<int, List<MonsterRecord>> RecordDic = new Dictionary<int, List<MonsterRecord>>();

        public void Init()
        {
        }
		public void Dispose()
		{
            Dictionary<string, Monster> monsterDic = GetCurrentDic();
            foreach(Monster monster in monsterDic.Values)
            {
                Game.Instance.AICore.RemoveAI(monster.Uid);
                Monster.Recycle(monster);
            }
            monsterDic.Clear();
            Adam.Instance.ClearTarget();
		}
        public void SaveRecord()
        {
            int hallKid = Hall.IsActive ? Hall.Instance.Data.Kid : 0;
            Dictionary<string, Monster> monsterDic = GetCurrentDic();

            List<MonsterRecord> recordList = null;
            if(RecordDic.ContainsKey(hallKid))
            {
                recordList = RecordDic[hallKid];
                recordList.Clear();
            }
            else
            {
                recordList = new List<MonsterRecord>();
                RecordDic[hallKid] = recordList;
            }
            foreach(Monster monster in monsterDic.Values)
            {
                if (monster.Info.IsAlive)
                    recordList.Add(monster.ToRecord());
            }
        }

        public Monster GetMonster(string uid)
        {
            Dictionary<string, Monster> monsterDic = GetCurrentDic();
            if(!monsterDic.ContainsKey(uid))
            {
                BaseLogger.LogFormat("Can't find monster. uid={0}", uid);
            }
            return monsterDic[uid];
        }

        public Monster GetNearestMonster(Vector3 position, float maxSqrDistance)
        {
            return null;
        }

        #region Block

        public void Foreach(Action<Monster> action)
        {
            if (action == null) { return; }

            Dictionary<string, Monster> monsterDic = GetCurrentDic();
            var enumerator = monsterDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }
        public void ForeachInBlock(Action<Monster> action)
		{
			if (action == null) { return; }

            var enumerator = monsterBlockDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				action(enumerator.Current.Value);
			}
		}
        public void ForeachInHall(Action<Monster> action)
        {
            if (action == null) { return; }

            var enumerator = monsterHallDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }

		public void AddMonster(Monster monster)
		{
            Dictionary<string, Monster> monsterDic = GetCurrentDic();
            if (!monsterDic.ContainsKey(monster.Uid))
			{
                monsterDic.Add(monster.Uid, monster);
			}
            Game.Instance.AICore.AddAI(monster);
		}

		public void RemoveMonster(string uid)
		{
            Dictionary<string, Monster> monsterDic = GetCurrentDic();
            if (monsterDic.ContainsKey(uid))
			{
                Monster monster = monsterDic[uid];
                monsterDic.Remove(uid);

                Game.Instance.AICore.RemoveAI(monster.Uid);

//                if(Adam.Instance.TargetMonster == monster)
//                    Adam.Instance.ClearTarget();
			}
		}

        #endregion

        public static float GetMonsterRadius(MonsterSize size)
        {
            switch(size)
            {
                case MonsterSize.Small:
                    return GlobalConfig.MonsterConfig.SmallRadius;
                case MonsterSize.Medium:
                    return GlobalConfig.MonsterConfig.MediumRadius;
                case MonsterSize.Big:
                    return GlobalConfig.MonsterConfig.BigRadius;
            }
            return GlobalConfig.MonsterConfig.SmallRadius;
        }

        private Dictionary<string, Monster> GetCurrentDic()
        {
            return Hall.IsActive ? monsterHallDic : monsterBlockDic;
        }

	}
}

