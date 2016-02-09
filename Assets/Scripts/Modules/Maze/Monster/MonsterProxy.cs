using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using AI;

namespace GameLogic
{
	public class MonsterProxy : Proxy
	{
		public delegate void IterateFunc(Monster monster);

		//Active monsters in blocks.
        private Dictionary<string, Monster> monsterBlockDic = new Dictionary<string, Monster>();
        //Active monsters in blocks.
        private Dictionary<string, Monster> monsterHallDic = new Dictionary<string, Monster>();
		//Inactive monsters in blocks. Record them.
        private Dictionary<int, List<MonsterRecord>> recordBlockDic = new Dictionary<int, List<MonsterRecord>>();
        //Inactive monsters in halls. Record them.
        private Dictionary<int, List<MonsterRecord>> recordHallDic = new Dictionary<int, List<MonsterRecord>>();

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
//                monsterHallDic.Remove(uid);
            }
        }
        public void RemoveMonsterInHall(string uid, int hallKid)
        {
            if (monsterHallDic.ContainsKey(uid))
            {
                Monster monster = monsterHallDic[uid];
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

//                monsterHallDic.Remove(uid);
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

        public void InitRecordHallList(int hallKid)
        {
            recordHallDic[hallKid] = new List<MonsterRecord>();
        }
        public List<MonsterRecord> GetRecordHallList(int hallKid)
        {
            List<MonsterRecord> recordList = null;
            recordHallDic.TryGetValue(hallKid, out recordList);
            return recordList;
        }
	}
}

