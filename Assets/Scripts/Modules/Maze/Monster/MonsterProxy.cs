using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using Battle;

namespace GameLogic
{
	public class MonsterProxy : Proxy
	{
		public delegate void IterateFunc(Monster monster);

		//Active monsters in blocks.
        private Dictionary<string, Monster> monsterBlockDic = new Dictionary<string, Monster>();
        //Active monsters in hall.
        private Dictionary<string, Monster> monsterHallDic = new Dictionary<string, Monster>();
		//Inactive monsters in blocks. Record them.
        private Dictionary<int, List<MonsterRecord>> recordBlockDic = new Dictionary<int, List<MonsterRecord>>();
        //Inactive monsters in halls. Record them.
        private Dictionary<int, List<MonsterRecord>> recordHallDic = new Dictionary<int, List<MonsterRecord>>();

        public Dictionary<int, List<MonsterRecord>> RecordBlockDic
        {
            get { return recordBlockDic; }
            set { recordBlockDic = value; }
        }
        public Dictionary<int, List<MonsterRecord>> RecordHallDic
        {
            get { return recordHallDic; }
            set { recordHallDic = value; }
        }

		public void Dispose()
		{
            ClearMonstersInBlock();
            ClearMonstersInHall();

            Dictionary<int, List<MonsterRecord>>.Enumerator recordEnum;
            recordEnum = recordBlockDic.GetEnumerator();
            while (recordEnum.MoveNext())
            {
                recordEnum.Current.Value.Clear();
            }
            recordBlockDic.Clear();
            recordEnum = recordHallDic.GetEnumerator();
            while (recordEnum.MoveNext())
            {
                recordEnum.Current.Value.Clear();
            }
            recordHallDic.Clear();
		}

        public Monster GetNearestMonster(Vector3 position, float maxSqrDistance)
        {
            Dictionary<string, Monster> monsterDic = Hero.Instance.Info.IsInHall ? monsterHallDic : monsterBlockDic;
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

		public void IterateMonstersInBlocks(IterateFunc func)
		{
			if (func == null) { return; }

			Dictionary<string, Monster>.Enumerator enumerator = monsterBlockDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				func(enumerator.Current.Value);
			}
		}

		public void AddMonsterInBlock(Monster monster)
		{
			if (!monsterBlockDic.ContainsKey(monster.Uid))
			{
				monsterBlockDic.Add(monster.Uid, monster);
			}
		}

		public void HideMonsterInBlock(string uid)
		{
			if (monsterBlockDic.ContainsKey(uid))
			{
				Monster monster = monsterBlockDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				List<MonsterRecord> recordList = GetRecordBlockList((int)mazePos.x, (int)mazePos.y);
				recordList.Add(monster.ToRecord());
				monsterBlockDic.Remove(uid);
			}
		}
		
		public void RemoveMonsterInBlock(string uid)
		{
			if (monsterBlockDic.ContainsKey(uid))
			{
				Monster monster = monsterBlockDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				List<MonsterRecord> recordList = GetRecordBlockList((int)mazePos.x, (int)mazePos.y);
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

				monsterBlockDic.Remove(uid);
			}
		}
        public void ClearMonstersInBlock()
        {
            Dictionary<string, Monster>.Enumerator monsterEnum = monsterBlockDic.GetEnumerator();
            while (monsterEnum.MoveNext())
            {
                Monster.Recycle(monsterEnum.Current.Value);
            }
            monsterBlockDic.Clear();
        }

        public void InitRecordBlockList(int blockKey)
		{
            recordBlockDic[blockKey] = new List<MonsterRecord>();
		}
		public List<MonsterRecord> GetRecordBlockList(int blockKey)
		{
			List<MonsterRecord> recordList = null;
			recordBlockDic.TryGetValue(blockKey, out recordList);
			return recordList;
		}
		public List<MonsterRecord> GetRecordBlockList(int col, int row)
		{
			Block block = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>().GetBlockAtPosition(col, row);
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			return GetRecordBlockList(blockKey);
		}
        /// <summary>
        /// Record all the active monsters in blocks. Other inactive monsters have been recorded when they hide.
        /// </summary>
        public void DoRecordBlock()
        {
            BlockProxy blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
            blockProxy.Iterate((Block block) =>
                {
                    int blockKey = Block.GetBlockKey(block.Col, block.Row);
                    InitRecordBlockList(blockKey);
                });
            var enumerator = monsterBlockDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Monster monster = enumerator.Current.Value;
                if(monster.Info.IsAlive)
                {
                    Vector2 pos = monster.MazePosition;
                    List<MonsterRecord> recordList = GetRecordBlockList((int)pos.x, (int)pos.y);
                    recordList.Add(monster.ToRecord());
                }
            }
        }

        #endregion

        #region Hall

        public void IterateMonstersInHall(IterateFunc func)
        {
            if (func == null) { return; }

            Dictionary<string, Monster>.Enumerator enumerator = monsterHallDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
        }
        public void AddMonsterInHall(Monster monster)
        {
            if(!monsterHallDic.ContainsKey(monster.Uid))
            {
                monsterHallDic.Add(monster.Uid, monster);
            }
        }
        public void HideMonsterInHall(string uid, int hallKid)
        {
            if (monsterHallDic.ContainsKey(uid))
            {
                Monster monster = monsterHallDic[uid];
                List<MonsterRecord> recordList = GetRecordHallList(hallKid);
                recordList.Add(monster.ToRecord());
            }
        }
        public void RemoveMonsterInHall(string uid, int hallKid)
        {
            if (monsterHallDic.ContainsKey(uid))
            {
                List<MonsterRecord> recordList = GetRecordHallList(hallKid);
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
            }
        }
        public void ClearMonstersInHall()
        {
            Dictionary<string, Monster>.Enumerator monsterEnum = monsterHallDic.GetEnumerator();
            while (monsterEnum.MoveNext())
            {
                Monster.Recycle(monsterEnum.Current.Value);
            }
            monsterHallDic.Clear();
        }

        public List<MonsterRecord> InitRecordHallList(int hallKid)
        {
            List<MonsterRecord> list = new List<MonsterRecord>();
            recordHallDic[hallKid] = list;
            return list;
        }
        public List<MonsterRecord> GetRecordHallList(int hallKid)
        {
            List<MonsterRecord> recordList = null;
            recordHallDic.TryGetValue(hallKid, out recordList);
            return recordList;
        }
        public void DoRecordInHall()
        {
            var enumerator = monsterHallDic.GetEnumerator();
            List<MonsterRecord> recordList = InitRecordHallList(Hall.Instance.Data.Kid);
            while(enumerator.MoveNext())
            {
                Monster monster = enumerator.Current.Value;
                if(monster.Info.IsAlive)
                {
                    recordList.Add(monster.ToRecord());
                }
            }
        }

        #endregion


	}
}

