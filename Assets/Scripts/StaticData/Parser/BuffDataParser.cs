using UnityEngine;

using System;
using System.Collections.Generic;

namespace StaticData
{
    public class BuffDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, BuffData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, BuffData>();

            while(!EndOfRow)
            {
                int col = 0;
                BuffData data = new BuffData();

                data.Kid = ReadInt(col++);
                data.Type = ReadEnum<BuffType>(col++);
                data.SpecialType = ReadEnum<BuffSpecialType>(col++);
                data.Duration = ReadFloat(col++);
                data.AttributeRatioDic = ReadDictionary<int, float>(col++);
                data.AttributeRaiseDic = ReadDictionary<int, int>(col++);
                data.EmissionColor = ReadColor(col++);

                kvDic.Add(data.Kid, data);
                NextLine();
            }
        }
    }
}

