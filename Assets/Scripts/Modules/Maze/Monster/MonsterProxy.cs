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

		//Active monsters
		private Dictionary<string, Monster> monsterDic = new Dictionary<string, Monster>();
		//Inactive monsters. Record them.
		private Dictionary<int, List<MonsterRecord>> recordDic = new Dictionary<int, List<MonsterRecord>>();

		public void Dispose()
		{
			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Monster.Recycle(enumerator.Current.Value);
			}
			monsterDic.Clear();
		}

		public void IterateMonsters(IterateFunc func)
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
				Vector2 mazePos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				List<MonsterRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
				recordList.Add(monster.ToRecord());

				monsterDic.Remove(uid);
			}
		}
		
		public void RemoveMonster(string uid)
		{
			if (monsterDic.ContainsKey(uid))
			{
				Monster monster = monsterDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				List<MonsterRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
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
			}

		}

		public void InitRecordList(int blockKey)
		{
			recordDic[blockKey] = new List<MonsterRecord>();
		}
		public List<MonsterRecord> GetRecordList(int blockKey)
		{
			List<MonsterRecord> recordList = null;
			recordDic.TryGetValue(blockKey, out recordList);
			return recordList;
		}
		public List<MonsterRecord> GetRecordList(int col, int row)
		{
			Block block = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>().GetBlockAtPosition(col, row);
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			return GetRecordList(blockKey);
		}
	}
}

