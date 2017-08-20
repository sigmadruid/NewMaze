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
        public float UseInterval;

        public ItemInfo(ItemData data, int count)
        {
            Data = data;
            Count = count;
            UseInterval = 0;
        }

        public ItemRecord ToRecord()
        {
            ItemRecord record = new ItemRecord();
            record.Kid = data.Kid;
            record.Count = Count;
            record.UseInterval = UseInterval;
            return record;
        }

        public bool CanUse
        {
            get
            {
                return UseInterval <= 0;
            }
        }

        //TODO: Factory Method
    }
}

