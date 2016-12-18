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
            //Mock data
            ItemData data;
            ItemInfo info;

            data = ItemDataManager.Instance.GetData(140001) as ItemData;
            info = new ItemInfo(data, 1234);
            itemInfoDic.Add(140001, info);

            data = ItemDataManager.Instance.GetData(140002) as ItemData;
            info = new ItemInfo(data, 56);
            itemInfoDic.Add(140002, info);

            data = ItemDataManager.Instance.GetData(140003) as ItemData;
            info = new ItemInfo(data, 234);
            itemInfoDic.Add(140003, info);
        }

        public void Dispose()
        {
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


    }
}