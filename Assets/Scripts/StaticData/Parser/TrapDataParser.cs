using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class TrapDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, TrapData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, TrapData>();


            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    TrapData data = new TrapData();

                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.Attack = StaticReader.ReadFloat(GetContent(col++));

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

