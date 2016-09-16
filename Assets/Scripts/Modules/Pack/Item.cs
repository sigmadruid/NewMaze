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
        public new ItemData Data
        {
            get { return data as ItemData; }
            protected set { data = value; }
        }

        public new ItemScript Script
        {
            get { return script as ItemScript; }
            protected set { script = value; }
        }

        public int Count;

        public void PickedUp()
        {
            TopAlertPanel.AddAlert(string.Format("Pick up coins: {0}", Count));
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
            record.Count = Count;
            return record;
        }

        public static Item Create(ItemRecord record)
        {
            Item item = new Item();
            item.Uid = record.Uid;
            item.Data = ItemDataManager.Instance.GetData(record.Kid) as ItemData;
            item.Count = 1;
            item.Script = ResourceManager.Instance.LoadAsset<ItemScript>(ObjectType.GameObject, item.Data.GetResPath());
            item.Script.transform.parent = RootTransform.Instance.DropRoot; 

            item.SetPosition(record.WorldPosition.ToVector3());

            return item;
        }
        public static Item Create(int kid, Vector3 position)
        {
            Item item = new Item();
            item.Uid = Guid.NewGuid().ToString();
            item.Data = ItemDataManager.Instance.GetData(kid) as ItemData;
            item.Count = 1;
            item.Script = ResourceManager.Instance.LoadAsset<ItemScript>(ObjectType.GameObject, item.Data.GetResPath());
            item.Script.transform.parent = RootTransform.Instance.DropRoot; 

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

