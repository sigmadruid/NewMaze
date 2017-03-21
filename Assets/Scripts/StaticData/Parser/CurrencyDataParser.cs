using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class CurrencyDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, CurrencyData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

			
			kvDic = new Dictionary<int, CurrencyData>();
			
            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    CurrencyData data = new CurrencyData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Desc = StaticReader.ReadString(GetContent(col++));
                    data.Res2D = StaticReader.ReadString(GetContent(col++));
                    data.Res3DList = StaticReader.ReadStringList(GetContent(col++));
                    kvDic.Add(data.Kid, data);

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

