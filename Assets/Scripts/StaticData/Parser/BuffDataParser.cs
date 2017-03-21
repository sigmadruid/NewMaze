using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class BuffDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, BuffData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, BuffData>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    BuffData data = new BuffData();

                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Type = StaticReader.ReadEnum<BuffType>(GetContent(col++));
                    data.SpecialType = StaticReader.ReadEnum<BuffSpecialType>(GetContent(col++));
                    data.Duration = StaticReader.ReadFloat(GetContent(col++));
                    data.AttributeRatioDic = StaticReader.ReadDictionary<int, float>(GetContent(col++));
                    data.AttributeRaiseDic = StaticReader.ReadDictionary<int, int>(GetContent(col++));
                    data.EmissionColor = StaticReader.ReadColor(GetContent(col++));

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

