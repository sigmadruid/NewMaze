using System;
using Base;

namespace StaticData
{
    public class WeaponData : EntityData
    {
        public string Name;

        public string Res3D;


        public override string GetResPath ()
        {
            if (resPath == null)
            {
                resPath = Utils.CombinePath("Weapons", Res3D);
            }
            return resPath;
        }
    }
}

