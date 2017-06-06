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
        public Dictionary<int, List<ItemRecord>> RecordDic = new Dictionary<int, List<ItemRecord>>();

        public void Init()
        {
        }
		public void Dispose()
		{
            ClearItems();
		}
        public Dictionary<int, List<ItemRecord>> CreateRecord()
        {
            int location;
            if(Hall.Instance != null)
            {
                location = Maze.GetCurrentLocation(Hall.Instance.Data.Kid);
                InitRecordList(Hall.Instance);
            }
            else
            {
                BlockProxy blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
                blockProxy.ForeachBlocks((Block block) =>
                    {
                        InitRecordList(block);
                    });
            }
            var enumerator = itemDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Item item = enumerator.Current.Value;
                location = Maze.GetCurrentLocation(item.WorldPosition);
                List<ItemRecord> recordList = GetRecordList(location);
                recordList.Add(item.ToRecord());
            }
            return RecordDic;
        }
        public void SetRecord(Dictionary<int, List<ItemRecord>> recordDic)
        {
            RecordDic = recordDic;
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
		
        public Item GetItemByUid(string uid)
        {
            if(!itemDic.ContainsKey(uid))
            {
                BaseLogger.Log("can't find item with uid: " + uid);
            }
            return itemDic[uid];
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
                int location = Maze.GetCurrentLocation(item.WorldPosition);
                List<ItemRecord> recordList = GetRecordList(location);
				recordList.Add(item.ToRecord() as ItemRecord);
				itemDic.Remove(uid);
                Item.Recycle(item);
			}
		}

		public void RemoveItem(string uid)
		{
			if (itemDic.ContainsKey(uid))
			{
                Item item = itemDic[uid];
                int location = Maze.GetCurrentLocation(item.WorldPosition);
                List<ItemRecord> recordList = GetRecordList(location);
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
                Item.Recycle(item);
			}
		}

        public void ClearItems()
        {
            Dictionary<string, Item>.Enumerator enumerator = itemDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item.Recycle(enumerator.Current.Value);
            }
            itemDic.Clear();
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

        public void InitRecordList(Block block)
		{
            if(block.IsRoom)
            {
                block.ForeachNode((MazePosition mazePos) =>
                    {
                        int location = Maze.GetCurrentLocation(mazePos.Col, mazePos.Row);
                        RecordDic[location] = new List<ItemRecord>();
                    });
            }
            else
            {
                int location = Maze.GetCurrentLocation(block.Col, block.Row);
                RecordDic[location] = new List<ItemRecord>();
            }
        }
        public void InitRecordList(Hall hall)
        {
            int location = Maze.GetCurrentLocation(hall.Data.Kid);
            RecordDic[location] = new List<ItemRecord>();
        }
        public List<ItemRecord> GetRecordList(int location)
		{
			List<ItemRecord> recordList = null;
            RecordDic.TryGetValue(location, out recordList);
			return recordList;
		}
    }
}

