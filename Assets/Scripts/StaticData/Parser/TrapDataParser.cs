using System;
using System.Collections.Generic;

namespace StaticData
{
    public class TrapDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, TrapData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, TrapData>();

            while(!EndOfRow)
            {
                int col = 0;
                TrapData data = new TrapData();

                data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
                data.Res3D = ReadString(col++);
                data.Attack = ReadFloat(col++);

                kvDic.Add(data.Kid, data);
                NextLine();
            }
        }
    }
}

