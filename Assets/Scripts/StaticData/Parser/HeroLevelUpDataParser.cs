using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class HeroLevelUpDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, HeroLevelUpData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, HeroLevelUpData>();

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    HeroLevelUpData data = new HeroLevelUpData();
                    data.Level= StaticReader.ReadInt(GetContent(col++));
                    data.Exp = StaticReader.ReadInt(GetContent(col++));

                    kvDic.Add(data.Level, data);
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

