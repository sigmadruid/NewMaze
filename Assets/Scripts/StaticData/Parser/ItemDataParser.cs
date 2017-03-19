using System;
using System.Collections.Generic;

namespace StaticData
{
    public class ItemDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, ItemData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, ItemData>();

            while(!EndOfRow)
            {
                int col = 0;
                ItemData data = new ItemData();

                data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
                data.Description = ReadString(col++);
                data.Type = ReadEnum<ItemType>(col++);
                data.Res2D = ReadString(col++);
                data.Res3D = ReadString(col++);

                kvDic.Add(data.Kid, data);
                NextLine();
            }
        }
    }
}
