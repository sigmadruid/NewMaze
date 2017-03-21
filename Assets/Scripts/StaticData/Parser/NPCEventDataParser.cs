using System;
using System.Collections.Generic;

using Base;

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

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    NPCEventData data = new NPCEventData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.AppearScene = StaticReader.ReadEnum<NPCAppearScene>(GetContent(col++));
                    data.Type = StaticReader.ReadEnum<NPCEventType>(GetContent(col++));
                    data.FirstTalkList = StaticReader.ReadStringList(GetContent(col++));
                    data.InTaskTalkList = StaticReader.ReadStringList(GetContent(col++));
                    data.FinishTalkList = StaticReader.ReadStringList(GetContent(col++));
                    data.EndTalkList = StaticReader.ReadStringList(GetContent(col++));

                    typeDic[data.AppearScene].Add(data);
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

