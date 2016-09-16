using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class DropProxy : Proxy
    {
        public delegate void IterateFunc(Item item);

        private Dictionary<string, Item> itemDic = new Dictionary<string, Item>();
		private Dictionary<int, List<ItemRecord>> recordDic = new Dictionary<int, List<ItemRecord>>();

		public void Dispose()
		{
            Dictionary<string, Item>.Enumerator enumerator = itemDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
                Item.Recycle(enumerator.Current.Value);
			}
			itemDic.Clear();
		}

        public static ItemInfo GenerateItemInfoByDrop(DropData dropData)
        {
            int index = RandomUtils.Weight(dropData.WeightList);
            int itemKid = dropData.ItemKidList[index];
            int count = RandomUtils.Range(dropData.MinCountList[index], dropData.MaxCountList[index]);
            ItemData data = ItemDataManager.Instance.GetData(itemKid) as ItemData;
            ItemInfo info = new ItemInfo(data, count);
            return info;
        }
		
		public void IterateDrops(IterateFunc func)
		{
			if (func == null) { return; }
			
            Dictionary<string, Item>.Enumerator enumerator = itemDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				func(enumerator.Current.Value);
			}
		}
		
        public void AddItem(Item item)
		{
			if (!itemDic.ContainsKey(item.Uid))
			{
				itemDic.Add(item.Uid, item);
			}
		}

		public void HideItem(string uid)
		{
			if (itemDic.ContainsKey(uid))
			{
                Item item = itemDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(item.WorldPosition);
				List<ItemRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
				recordList.Add(item.ToRecord() as ItemRecord);

				itemDic.Remove(uid);
			}
		}

		public void RemoveItem(string uid)
		{
			if (itemDic.ContainsKey(uid))
			{
                Item item = itemDic[uid];
				Vector2 mazePos = Maze.Instance.GetMazePosition(item.WorldPosition);
				List<ItemRecord> recordList = GetRecordList((int)mazePos.x, (int)mazePos.y);
				if (recordList != null)
				{
					for (int i = 0; i < recordList.Count; ++i)
					{
						ItemRecord record = recordList[i];
						if (record.Uid == uid)
						{
							recordList.RemoveAt(i);
							break;
						}
					}
				}

				itemDic.Remove(uid);
			}
		}

        public Item FindNearbyItem(Vector3 position)
		{
            foreach(Item item in itemDic.Values)
			{
				if (Vector3.SqrMagnitude(item.WorldPosition - position) < GlobalConfig.DropConfig.NearSqrDistance)
				{
					return item;
				}
			}
			return null;
		}

		public void InitRecordList(int blockKey)
		{
			recordDic[blockKey] = new List<ItemRecord>();
		}
		public List<ItemRecord> GetRecordList(int blockKey)
		{
			List<ItemRecord> recordList = null;
			recordDic.TryGetValue(blockKey, out recordList);
			return recordList;
		}
		public List<ItemRecord> GetRecordList(int col, int row)
		{
			Block block = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>().GetBlockAtPosition(col, row);
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			return GetRecordList(blockKey);
		}
    }
}

