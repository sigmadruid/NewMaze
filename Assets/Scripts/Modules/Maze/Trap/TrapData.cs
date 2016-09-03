using System;

using Base;

namespace StaticData
{
    public class TrapData : EntityData
    {
        public string Name;

        public string Res3D;

        public int Attack;

        public override string GetResPath ()
        {
            if (resPath == null)
            {
                resPath = Utils.CombinePath("Traps", Res3D);
            }
            return resPath;
        }
    }
}
