using System;
using System.Collections.Generic;

namespace StaticData
{
    public class TextDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<string, string> kvDic)
		{
            LoadFile(CONFIG_PATH + name);
			
            kvDic = new Dictionary<string, string>();
			
			while(!EndOfRow)
			{
				int col = 0;
                string id = ReadString(col++);
				string text = ReadString(col++);
				kvDic.Add(id, text);
				NextLine();
			}
		}
    }
}

