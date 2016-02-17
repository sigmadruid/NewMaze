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
            LoadFile(name);

            kvDic = new Dictionary<int, KeyboardData>();

            while(!EndOfRow)
            {
                int col = 0;
                KeyboardData data = new KeyboardData();

                data.Type = ReadEnum<KeyboardActionType>(col++);
                data.Key = ReadEnum<KeyCode>(col++);
                data.Name = ReadString(col++);
                data.ActiveWhenPause = ReadBool(col++);

                kvDic.Add((int)data.Type, data);
                NextLine();
            }
        }
    }
}

