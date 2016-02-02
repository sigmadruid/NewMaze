using System;
using System.Collections.Generic;

namespace StaticData
{
    public class BulletDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, BulletData> kvDic)
        {
			LoadFile(name);
			
			kvDic = new Dictionary<int, BulletData>();

			while(!EndOfRow)
			{
				int col = 0;
				BulletData data = new BulletData();

				data.Kid = ReadInt(col++);
				data.Name = ReadString(col++);
				data.Res3D = ReadString(col++);
				data.Speed = ReadFloat(col++);
				data.Radius = ReadFloat(col++);

				kvDic.Add(data.Kid, data);
				NextLine();
			}
        }
    }
}

