using System;
using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class UIDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<string, UIData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<string, UIData>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    UIData data = new UIData();

                    data.UIName = StaticReader.ReadString(GetContent(col++));
                    data.PrefabPath = StaticReader.ReadString(GetContent(col++));
                    data.QueueMode = StaticReader.ReadEnum<PopupQueueMode>(GetContent(col++));

                    kvDic.Add(data.UIName, data);
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

