using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameUI;
using StaticData;

namespace GameLogic
{
    public class Item : Entity
    {
        public new ItemScript Script
        {
            get { return script as ItemScript; }
            protected set { script = value; }
        }

        public new ItemData Data
        {
            get { return data as ItemData; }
            protected set { data = value; }
        }

        public new ItemInfo Info
        {
            get { return info as ItemInfo; }
            protected set { info = value; }
        }

        public void PickedUp()
        {
//            TopAlertPanel.AddAlert(string.Format("Pick up coins: {0}", Info.Count));
        }

        public void StartFlying(Vector3 position)
        {
            Script.Fly(position);
        }

        public override EntityRecord ToRecord()
        {
            ItemRecord record = new ItemRecord();
            record.Kid = Data.Kid;
            record.WorldPosition = new Vector3Record(WorldPosition);
            record.ItemKid = Data.Kid;
            return record;
        }

        public static Item Create(ItemRecord record)
        {
            Item item = new Item();
            item.Uid = record.Uid;
            item.Data = ItemDataManager.Instance.GetData(record.Kid) as ItemData;
            item.Info = new ItemInfo(item.Data, record.Count);
            item.Script = ResourceManager.Instance.LoadAsset<ItemScript>(ObjectType.GameObject, item.Data.GetResPath());
            item.Script.Uid = item.Uid;
            item.Script.transform.parent = RootTransform.Instance.DropRoot; 

            item.SetPosition(record.WorldPosition.ToVector3());

            return item;
        }
        public static Item Create(DropData dropData, Vector3 position)
        {
            ItemInfo info = DropProxy.GenerateItemInfoByDrop(dropData);

            Item item = new Item();
            item.Uid = Guid.NewGuid().ToString();
            item.Data = info.Data;
            item.Info = info;
            item.Script = ResourceManager.Instance.LoadAsset<ItemScript>(ObjectType.GameObject, item.Data.GetResPath());
            item.Script.transform.parent = RootTransform.Instance.DropRoot; 
            item.Script.Uid = item.Uid;

            item.SetPosition(position);

            return item;
        }

        public static void Recycle(Item drop)
        {
            if (drop != null)
            {
                ResourceManager.Instance.RecycleAsset(drop.Script.gameObject);
                drop.Script = null;
                drop.Data = null;
            }
            else
            {
                BaseLogger.Log("Recyle a null item!");
            }
        }
    }
}

