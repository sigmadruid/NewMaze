using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class DropProxy : Proxy
    {
        private Dictionary<string, Item> itemBlockDic = new Dictionary<string, Item>();
        private Dictionary<string, Item> itemHallDic = new Dictionary<string, Item>();

        public Dictionary<int, List<ItemRecord>> RecordDic = new Dictionary<int, List<ItemRecord>>();

        public void Init()
        {
        }
		public void Dispose()
		{
            Dictionary<string, Item> itemDic = GetCurrentDic();
            var enumerator = itemDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item.Recycle(enumerator.Current.Value);
            }
            itemDic.Clear();
		}
        public void SaveRecord()
        {
            int hallKid = Hall.IsActive ? Hall.Instance.Data.Kid : 0;
            Dictionary<string, Item> itemDic = GetCurrentDic();

            List<ItemRecord> recordList = null;
            if(RecordDic.ContainsKey(hallKid))
            {
                recordList = RecordDic[hallKid];
                recordList.Clear();
            }
            else
            {
                recordList = new List<ItemRecord>();
                RecordDic[hallKid] = recordList;
            }
            foreach(Item item in itemDic.Values)
            {
                recordList.Add(item.ToRecord());
            }
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
		
        public void ForeachDrops(Action<Item> action)
		{
			if (action == null) { return; }
			
            Dictionary<string, Item> itemDic = GetCurrentDic();
            var enumerator = itemDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
		}
		
        public Item GetItemByUid(string uid)
        {
            Dictionary<string, Item> itemDic = GetCurrentDic();
            if(!itemDic.ContainsKey(uid))
            {
                BaseLogger.Log("can't find item with uid: " + uid);
            }
            return itemDic[uid];
        }

        public void AddItem(Item item)
		{
            Dictionary<string, Item> itemDic = GetCurrentDic();
			if (!itemDic.ContainsKey(item.Uid))
			{
				itemDic.Add(item.Uid, item);
			}
		}

		public void RemoveItem(string uid)
		{
            Dictionary<string, Item> itemDic = GetCurrentDic();
			if (itemDic.ContainsKey(uid))
			{
                Item item = itemDic[uid];
				itemDic.Remove(uid);
			}
		}

        public Item FindNearbyItem(Vector3 position)
		{
            Dictionary<string, Item> itemDic = GetCurrentDic();
            foreach(Item item in itemDic.Values)
			{
				if (Vector3.SqrMagnitude(item.WorldPosition - position) < GlobalConfig.DropConfig.NearSqrDistance)
				{
					return item;
				}
			}
			return null;
		}

        private Dictionary<string, Item> GetCurrentDic()
        {
            return Hall.IsActive ? itemHallDic : itemBlockDic;
        }
    }
}

