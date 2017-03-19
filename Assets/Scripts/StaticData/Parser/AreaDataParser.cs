using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class AreaDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, AreaData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, AreaData>();

            while (!EndOfRow)
            {
                int col = 0;

                AreaData data = new AreaData();
                data.Kid = ReadInt(col++);
                data.AreaType = ReadEnum<AreaType>(col++);
                data.Param1 = ReadFloat(col++);
                data.Param2 = ReadFloat(col++);

                kvDic.Add(data.Kid, data);

                NextLine();
            }
        }
    }
}

