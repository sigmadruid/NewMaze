using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class PackProxy : Proxy
    {
        private Dictionary<int, ItemInfo> itemInfoDic = new Dictionary<int, ItemInfo>();

        public void Init()
        {
        }

        public void Dispose()
        {
        }

        public Dictionary<int, ItemRecord> GetRecord()
        {
            Dictionary<int, ItemRecord> recordDic = new Dictionary<int, ItemRecord>();
            foreach(int kid in itemInfoDic.Keys)
            {
                ItemInfo info = itemInfoDic[kid];
                ItemRecord record = info.ToRecord();
                recordDic.Add(kid, record);
            }
            return recordDic;
        }
        public void SetRecord(Dictionary<int, ItemRecord> recordDic)
        {
            itemInfoDic.Clear();
            foreach(int kid in recordDic.Keys)
            {
                ItemRecord record = recordDic[kid];
                ChangeCount(kid, record.Count);
            }
        }

        public ItemInfo GetItemInfo(int kid)
        {
            if(itemInfoDic.ContainsKey(kid))
            {
                return itemInfoDic[kid];
            }
            return null;
        }

        public bool HasCount(int kid, int count)
        {
            if(itemInfoDic.ContainsKey(kid))
            {
                ItemInfo info = itemInfoDic[kid];
                return info.Count >= count;
            }
            return false;
        }

        public void ChangeCount(int kid, int count)
        {
            ItemInfo info = null;
            if(itemInfoDic.ContainsKey(kid))
            {
                info = itemInfoDic[kid];
                info.Count += count;
                info.Count = Mathf.Max(info.Count, 0);
                if(info.Count <= 0)
                {
                    itemInfoDic.Remove(kid);
                }
            }
            else if(count > 0)
            {
                ItemData data = ItemDataManager.Instance.GetData(kid) as ItemData;
                info = new ItemInfo(data, count);
                itemInfoDic[kid] = info;
            }
            else
            {
                BaseLogger.LogFormat("Illegal item count manuplation: kid={0}, count={1}", kid, count);
            }
            DispatchNotification(NotificationEnum.PACK_CHANGE_ITEM_COUNT, kid);
        }

        public List<ItemInfo> GetItemInfosByType(ItemType type)
        {
            List<ItemInfo> result = new List<ItemInfo>();
            var enumerator = itemInfoDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                ItemInfo info = enumerator.Current.Value;
                if(info.Data.Type == type)
                {
                    result.Add(info);
                }
            }
            return result;
        }

        public void Use(int kid)
        {
            if(HasCount(kid, 1))
            {
                ChangeCount(kid, -1);
                DispatchNotification(NotificationEnum.PACK_USE_ITEM, kid);
            }
            else
            {
                BaseLogger.LogFormat("No item to use: kid={0}", kid);
            }
        }

    }
}