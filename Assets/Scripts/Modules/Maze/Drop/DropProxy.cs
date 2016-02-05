using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class DropProxy : Proxy
    {
		public delegate void IterateFunc(Drop drop);

		private Dictionary<string, Drop> dropDic = new Dictionary<string, Drop>();
		private Dictionary<int, List<DropRecord>> recordDic = new Dictionary<int, List<DropRecord>>();

		public void Dispose()
		{
			Dictionary<string, Drop>.Enumerator enumerator = dropDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Drop.Recycle(enumerator.Current.Value);
			}
			dropDic.Clear();
		}
		
		public void IterateDrops(IterateFunc func)
		{
			if (func == null) { return; }
			
			Dictionary<string, Drop>.Enumerator enumerator = dropDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				func(enumerator.Current.Value);
			}
		}
		
		public void AddDrop(Drop drop)
		{
			if (!dropDic.ContainsKey(drop.Uid))
			{
				dropDic.Add(drop.Uid, drop);
			}
		}

		public void HideDrop(string uid)
		{
			if (dropDic.ContainsKey(uid))
			{
				Drop drop = dropDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(drop.WorldPosition);
				List<DropRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
				recordList.Add(drop.ToRecord() as DropRecord);

				dropDic.Remove(uid);
			}
		}

		public void RemoveDrop(string uid)
		{
			if (dropDic.ContainsKey(uid))
			{
				Drop drop = dropDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(drop.WorldPosition);
				List<DropRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
				if (recordList != null)
				{
					for (int i = 0; i < recordList.Count; ++i)
					{
						DropRecord record = recordList[i];
						if (record.Uid == uid)
						{
							recordList.RemoveAt(i);
							break;
						}
					}
				}

				dropDic.Remove(uid);
			}
		}

		public Drop FindNearbyDrop(Vector3 position)
		{
			foreach(Drop drop in dropDic.Values)
			{
				if (Vector3.SqrMagnitude(drop.WorldPosition - position) < GlobalConfig.InputConfig.NearSqrDistance)
				{
					return drop;
				}
			}
			return null;
		}

		public void InitRecordList(int blockKey)
		{
			recordDic[blockKey] = new List<DropRecord>();
		}
		public List<DropRecord> GetRecordList(int blockKey)
		{
			List<DropRecord> recordList = null;
			recordDic.TryGetValue(blockKey, out recordList);
			return recordList;
		}
		public List<DropRecord> GetRecordList(int col, int row)
		{
			Block block = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>().GetBlockAtPosition(col, row);
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			return GetRecordList(blockKey);
		}
    }
}

