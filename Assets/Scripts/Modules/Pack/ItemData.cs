using System;

using Base;

namespace StaticData
{
    public enum ItemType
    {
        Resource = 0,
        Rune = 1,
        None,
    }

    public class ItemData : EntityData
    {
        public string Name;
        public string Description;
        public ItemType Type;

        public string Res2D;
        public string Res3D;

        public float UseInterval;

        public string Param1;
        public string Param2;
        public string Param3;

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
