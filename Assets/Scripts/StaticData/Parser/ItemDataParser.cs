using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class ItemDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, ItemData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, ItemData>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    ItemData data = new ItemData();

                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Description = StaticReader.ReadString(GetContent(col++));
                    data.Type = StaticReader.ReadEnum<ItemType>(GetContent(col++));
                    data.Res2D = StaticReader.ReadString(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.Param1 = StaticReader.ReadString(GetContent(col++));
                    data.Param2 = StaticReader.ReadString(GetContent(col++));
                    data.Param3 = StaticReader.ReadString(GetContent(col++));

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
