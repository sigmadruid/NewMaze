using System;
using System.Collections.Generic;

namespace StaticData
{
    public class CurrencyDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, CurrencyData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

			
			kvDic = new Dictionary<int, CurrencyData>();
			
			while (!EndOfRow)
			{
				int col = 0;
				
				CurrencyData data = new CurrencyData();
				data.Kid = ReadInt(col++);
				data.Name = ReadString(col++);
				data.Desc = ReadString(col++);
				data.Res2D = ReadString(col++);
				data.Res3DList = ReadStringList(col++);
				kvDic.Add(data.Kid, data);
				
				NextLine();
			}
        }
    }
}

