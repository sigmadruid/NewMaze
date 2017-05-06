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

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;

                    AreaData data = new AreaData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.AreaType = StaticReader.ReadEnum<AreaType>(GetContent(col++));
                    data.CenterBased = StaticReader.ReadBool(GetContent(col++));
                    data.Param1 = StaticReader.ReadFloat(GetContent(col++));
                    data.Param2 = StaticReader.ReadFloat(GetContent(col++));

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

