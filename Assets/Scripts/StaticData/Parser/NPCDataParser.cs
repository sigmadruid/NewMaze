using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class NPCDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<NPCAppearScene, List<NPCData>> typeDic, out Dictionary<int, NPCData> kvDic)
		{
            LoadFile(CONFIG_PATH + name);

			typeDic = new Dictionary<NPCAppearScene, List<NPCData>>();
			typeDic.Add(NPCAppearScene.HomeTown, new List<NPCData>());
			typeDic.Add(NPCAppearScene.Maze, new List<NPCData>());

			kvDic = new Dictionary<int, NPCData>();

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    NPCData data = new NPCData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.AppearScene = StaticReader.ReadEnum<NPCAppearScene>(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
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

