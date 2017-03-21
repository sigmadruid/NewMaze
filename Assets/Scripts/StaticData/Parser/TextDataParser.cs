using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class TextDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<string, string> kvDic)
		{
            LoadFile(CONFIG_PATH + name);
			
            kvDic = new Dictionary<string, string>();
			
            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    string id = StaticReader.ReadString(GetContent(col++));
                    string text = StaticReader.ReadString(GetContent(col++));
                    kvDic.Add(id, text);
                    NextLine();
                }
            }
            catch(Exception e)
            {
                col--;
                BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", GetContent(col), RowIndex, col, this.ToString());
            }
		}
    }
}

