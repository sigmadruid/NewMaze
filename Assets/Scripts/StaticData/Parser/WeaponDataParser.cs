using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class WeaponDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, WeaponData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, WeaponData>();


            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    WeaponData data = new WeaponData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));

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

