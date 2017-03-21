using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class KeyboardDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, KeyboardData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, KeyboardData>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    KeyboardData data = new KeyboardData();

                    data.Type = StaticReader.ReadEnum<KeyboardActionType>(GetContent(col++));
                    data.Key = StaticReader.ReadEnum<KeyCode>(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.ActiveWhenPause = StaticReader.ReadBool(GetContent(col++));

                    kvDic.Add((int)data.Type, data);
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

