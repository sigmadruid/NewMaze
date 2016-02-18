using System;
using System.Collections.Generic;

namespace StaticData
{
    public class ResourceDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, List<ResourceData>> typeDic)
		{
			LoadFile(name);

			typeDic = new Dictionary<int, List<ResourceData>>();

			while(!EndOfRow)
			{
				int col = 0;

				ResourceData data = new ResourceData();
				data.MazeKid = ReadInt(col++);
                data.EntityKid = ReadInt(col++);
                data.Path = ReadString(col++);
				data.Life = ReadInt(col++);
				data.PreloadCount = ReadInt(col++);

				if (!typeDic.ContainsKey(data.MazeKid))
				{
					typeDic.Add(data.MazeKid, new List<ResourceData>());
				}
				typeDic[data.MazeKid].Add(data);

				NextLine();
			}
		}
    }
}

