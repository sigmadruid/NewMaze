using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class DropDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, DropData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);
			
			kvDic = new Dictionary<int, DropData>();
			
            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    DropData data = new DropData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.MaxNum = StaticReader.ReadInt(GetContent(col++));
                    data.ItemKidList = StaticReader.ReadIntList(GetContent(col++));
                    data.MinCountList = StaticReader.ReadIntList(GetContent(col++));
                    data.MaxCountList = StaticReader.ReadIntList(GetContent(col++));
                    data.WeightList = StaticReader.ReadIntList(GetContent(col++));

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

