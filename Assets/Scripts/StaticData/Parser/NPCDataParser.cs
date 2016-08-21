using System;
using System.Collections.Generic;

namespace StaticData
{
	public class NPCDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<NPCAppearScene, List<NPCData>> typeDic, out Dictionary<int, NPCData> kvDic)
		{
			LoadFile(name);

			typeDic = new Dictionary<NPCAppearScene, List<NPCData>>();
			typeDic.Add(NPCAppearScene.HomeTown, new List<NPCData>());
			typeDic.Add(NPCAppearScene.Maze, new List<NPCData>());

			kvDic = new Dictionary<int, NPCData>();

			while (!EndOfRow)
			{
				int col = 0;

				NPCData data = new NPCData();
				data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
				data.AppearScene = ReadEnum<NPCAppearScene>(col++);
				data.Res3D = ReadString(col++);
				typeDic[data.AppearScene].Add(data);
				kvDic.Add(data.Kid, data);

				NextLine();
			}
		}
    }
}

