using System;

using Base;

namespace StaticData
{
    public enum ItemType
    {
        Resource,
        Rune,
        Equipment,
        SoulStone,
        Other,
    }

    public class ItemData : EntityData
    {
        public string Name;
        public string Description;
        public ItemType Type;
        public string Icon;

        public string Res2D;
        public string Res3D;

        public override string GetResPath()
        {
            if (resPath == null)
            {
                resPath = Utils.CombinePath("Items", Res3D);
            }
            return resPath;
        }
    }
}
