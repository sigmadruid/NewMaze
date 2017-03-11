using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class WeaponDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, WeaponData> kvDic)
        {
            LoadFile(name);

            kvDic = new Dictionary<int, WeaponData>();

            while (!EndOfRow)
            {
                int col = 0;

                WeaponData data = new WeaponData();
                data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
                data.Res3D = ReadString(col++);

                kvDic.Add(data.Kid, data);

                NextLine();
            }
        }
    }
}

