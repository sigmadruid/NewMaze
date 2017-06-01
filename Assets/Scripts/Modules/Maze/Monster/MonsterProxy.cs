using UnityEngine;

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
		public delegate void IterateFunc(Monster monster);

		//Active monsters.
        private Dictionary<string, Monster> monsterDic = new Dictionary<string, Monster>();
		//Inactive monsters. Record them.
        public Dictionary<int, List<MonsterRecord>> RecordDic = new Dictionary<int, List<MonsterRecord>>();

        public void Init()
        {
        }
		public void Dispose()
		{
            ClearMonsters();

            Dictionary<int, List<MonsterRecord>>.Enumerator recordEnum;
            recordEnum = RecordDic.GetEnumerator();
            while (recordEnum.MoveNext())
            {
                recordEnum.Current.Value.Clear();
            }
            RecordDic.Clear();
		}
        /// <summary>
        /// Record all the active monsters in blocks. Other inactive monsters have been recorded when they hide.
        /// </summary>
        public Dictionary<int, List<MonsterRecord>> CreateRecord()
        {
            int location;
            if(Hall.Instance != null)
            {
                InitRecordList(Hall.Instance);
            }
            else
            {
                BlockProxy blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
                blockProxy.Iterate((Block block) =>
                    {
                        InitRecordList(block);
                    });
            }
            var enumerator = monsterDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Monster monster = enumerator.Current.Value;
                if(monster.Info.IsAlive)
                {
                    location = Maze.GetCurrentLocation(monster.WorldPosition);
                    List<MonsterRecord> recordList = GetRecordList(location);
                    recordList.Add(monster.ToRecord());
                }
            }
            return RecordDic;
        }
        public void SetRecord(Dictionary<int, List<MonsterRecord>> recordDic)
        {
            RecordDic = recordDic;
        }

        public Monster GetMonster(string uid)
        {
            if(!monsterDic.ContainsKey(uid))
            {
                BaseLogger.LogFormat("Can't find monster. uid={0}", uid);
            }
            return monsterDic[uid];
        }

        public Monster GetNearestMonster(Vector3 position, float maxSqrDistance)
        {
            Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
            float sqrDistance = float.MaxValue;
            Monster result = null;
            while(enumerator.MoveNext())
            {
                Monster monster = enumerator.Current.Value;
                float newSqrDistance = (monster.WorldPosition - position).sqrMagnitude;
                if(newSqrDistance < sqrDistance && newSqrDistance < maxSqrDistance && monster.Info.IsAlive)
                {
                    sqrDistance = newSqrDistance;
                    result = monster;
                }
            }
            return result;
        }

        #region Block

		public void IterateActives(IterateFunc func)
		{
			if (func == null) { return; }

			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				func(enumerator.Current.Value);
			}
		}

		public void AddMonster(Monster monster)
		{
			if (!monsterDic.ContainsKey(monster.Uid))
			{
				monsterDic.Add(monster.Uid, monster);
			}
		}

		public void HideMonster(string uid)
		{
			if (monsterDic.ContainsKey(uid))
			{
				Monster monster = monsterDic[uid];
                int location = Maze.GetCurrentLocation(monster.WorldPosition);
                List<MonsterRecord> recordList = GetRecordList(location);
				recordList.Add(monster.ToRecord());
				monsterDic.Remove(uid);

                if(Adam.Instance.TargetMonster == monster)
                    Adam.Instance.ClearTarget();
			}
		}
		
		public void RemoveMonster(string uid)
		{
			if (monsterDic.ContainsKey(uid))
			{
				Monster monster = monsterDic[uid];
                int location = Maze.GetCurrentLocation(monster.WorldPosition);
                List<MonsterRecord> recordList = GetRecordList(location);
				if (recordList != null)
				{
					for (int i = 0; i < recordList.Count; ++i)
					{
						MonsterRecord record = recordList[i];
						if (record.Uid == uid)
						{
							recordList.RemoveAt(i);
							break;
						}
					}
				}
				monsterDic.Remove(uid);

                if(Adam.Instance.TargetMonster == monster)
                    Adam.Instance.ClearTarget();
			}
		}
        public void ClearMonsters()
        {
            Dictionary<string, Monster>.Enumerator monsterEnum = monsterDic.GetEnumerator();
            while (monsterEnum.MoveNext())
            {
                Monster.Recycle(monsterEnum.Current.Value);
            }
            monsterDic.Clear();
        }

        public void InitRecordList(Block block)
        {
            if(block.IsRoom)
            {
                block.ForeachNode((MazePosition mazePos) =>
                    {
                        int location = Maze.GetCurrentLocation(mazePos.Col, mazePos.Row);
                        RecordDic[location] = new List<MonsterRecord>();
                    });
            }
            else
            {
                int location = Maze.GetCurrentLocation(block.Col, block.Row);
                RecordDic[location] = new List<MonsterRecord>();
            }
        }
        public void InitRecordList(Hall hall)
		{
            int location = Maze.GetCurrentLocation(hall.Data.Kid);
            RecordDic[location] = new List<MonsterRecord>();
		}
		public List<MonsterRecord> GetRecordList(int location)
		{
			List<MonsterRecord> recordList = null;
            RecordDic.TryGetValue(location, out recordList);
			return recordList;
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

	}
}

