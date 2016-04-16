using UnityEngine;

using System;
using System.Collections.Generic;

namespace StaticData
{
    public class BuffDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, BuffData> kvDic)
        {
            LoadFile(name);

            kvDic = new Dictionary<int, BuffData>();

            while(!EndOfRow)
            {
                int col = 0;
                BuffData data = new BuffData();

                data.Kid = ReadInt(col++);
                data.Type = ReadEnum<BuffType>(col++);
                data.SpecialType = ReadEnum<BuffSpecialType>(col++);
                data.Duration = ReadFloat(col++);
                data.AttributeDic = ReadDictionary<int, float>(col, col + 1);

                kvDic.Add(data.Kid, data);
                NextLine();
            }
        }
    }
}

