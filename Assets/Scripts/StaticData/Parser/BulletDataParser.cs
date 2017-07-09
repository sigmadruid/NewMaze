using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class BulletDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, BulletData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);
			
			kvDic = new Dictionary<int, BulletData>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    BulletData data = new BulletData();

                    col = 0;
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.Speed = StaticReader.ReadFloat(GetContent(col++));
                    data.Radius = StaticReader.ReadFloat(GetContent(col++));
                    data.EndDuration = StaticReader.ReadFloat(GetContent(col++));
                    data.AddToTarget = StaticReader.ReadBool(GetContent(col++));
                    
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

