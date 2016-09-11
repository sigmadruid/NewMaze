using System;

using StaticData;

namespace GameLogic
{
    public class ItemInfo
    {
        public ItemData Data;

        public int Count;

        public ItemInfo(ItemData data, int count)
        {
            Data = data;
            Count = count;
        }

        //TODO: Factory Method
    }
}

