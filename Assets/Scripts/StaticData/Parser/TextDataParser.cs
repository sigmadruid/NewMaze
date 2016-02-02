using System;
using System.Collections.Generic;

namespace StaticData
{
    public class TextDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, string> kvDic)
		{
			LoadFile(name);
			
			kvDic = new Dictionary<int, string>();
			
			while(!EndOfRow)
			{
				int col = 0;
				int id = ReadInt(col++);
				string text = ReadString(col++);
				kvDic.Add(id, text);
				NextLine();
			}
		}
    }
}

