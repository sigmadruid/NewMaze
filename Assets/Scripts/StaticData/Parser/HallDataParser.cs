using System;
using System.Collections.Generic;

namespace StaticData
{
    public class HallDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, HallData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);
            
            kvDic = new Dictionary<int, HallData>();
            
            while(!EndOfRow)
            {
                int col = 0;
				HallData data = new HallData();
                
                data.Kid = ReadInt(col++);
                data.Res3D = ReadString(col++);
                
                kvDic.Add(data.Kid, data);
                NextLine();
            }
        }
    }
}

