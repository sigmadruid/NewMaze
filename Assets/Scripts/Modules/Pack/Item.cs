using System;

using Base;
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

        public static Item Create(int kid, int count)
        {
            Item item = new Item();
            item.Data = ItemDataManager.Instance.GetData(kid) as ItemData;
            item.Count = count;
            item.Script = ResourceManager.Instance.LoadAsset<ItemScript>(ObjectType.GameObject, item.Data.GetResPath());
            item.Script.transform.parent = RootTransform.Instance.DropRoot;
            return item;
        }

        public static void Recycle(Item item)
        {
            if(item != null)
            {
                item.Data = null;
                item.Script.StopAllCoroutines();
                ResourceManager.Instance.RecycleAsset(item.Script.gameObject);
                item.Script = null;
            }
            else
            {
                BaseLogger.Log("Recyle a null item!");
            }
        }
    }
}

