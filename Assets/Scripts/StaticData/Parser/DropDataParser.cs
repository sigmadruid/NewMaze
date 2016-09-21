using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class DropDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, DropData> kvDic)
        {
			LoadFile(name);
			
			kvDic = new Dictionary<int, DropData>();
			
			while (!EndOfRow)
			{
				int col = 0;
				
				DropData data = new DropData();
                data.Kid = ReadInt(col++);
                data.MaxNum = ReadInt(col++);
                data.ItemKidList = ReadIntList(col++);
                data.MinCountList = ReadIntList(col++);
                data.MaxCountList = ReadIntList(col++);
                data.WeightList = ReadIntList(col++);

				kvDic.Add(data.Kid, data);
				
				NextLine();
			}
        }
    }
}

