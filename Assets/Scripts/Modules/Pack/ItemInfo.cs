using System;

using StaticData;

namespace GameLogic
{
    public class ItemInfo : EntityInfo
    {
        public new ItemData Data
        {
            get { return data as ItemData; }
            set { data = value; }
        }

        public int Count;

        public ItemInfo(ItemData data, int count)
        {
            Data = data;
            Count = count;
        }

        //TODO: Factory Method
    }
}

