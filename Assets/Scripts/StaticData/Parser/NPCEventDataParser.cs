using System;
using System.Collections.Generic;

namespace StaticData
{
    public class NPCEventDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<NPCAppearScene, List<NPCEventData>> typeDic, out Dictionary<int, NPCEventData> kvDic)
		{
            LoadFile(CONFIG_PATH + name);

			typeDic = new Dictionary<NPCAppearScene, List<NPCEventData>>();
			typeDic.Add(NPCAppearScene.HomeTown, new List<NPCEventData>());
			typeDic.Add(NPCAppearScene.Maze, new List<NPCEventData>());

			kvDic = new Dictionary<int, NPCEventData>();

			while (!EndOfRow)
			{
				int col = 0;

				NPCEventData data = new NPCEventData();
				data.Kid = ReadInt(col++);
				data.AppearScene = ReadEnum<NPCAppearScene>(col++);
				data.Type = ReadEnum<NPCEventType>(col++);
                data.FirstTalkList = ReadStringList(col++);
                data.InTaskTalkList = ReadStringList(col++);
                data.FinishTalkList = ReadStringList(col++);
                data.EndTalkList = ReadStringList(col++);

				typeDic[data.AppearScene].Add(data);
				kvDic.Add(data.Kid, data);

				NextLine();
			}
		}
    }
}

