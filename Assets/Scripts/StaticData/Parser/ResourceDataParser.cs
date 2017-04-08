using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class ResourceDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, List<ResourceData>> typeDic)
		{
            LoadFile(CONFIG_PATH + name);

			typeDic = new Dictionary<int, List<ResourceData>>();

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;

                    ResourceData data = new ResourceData();
                    data.MazeKid = StaticReader.ReadInt(GetContent(col++));
                    data.EntityKid = StaticReader.ReadInt(GetContent(col++));
                    data.Path = StaticReader.ReadString(GetContent(col++));
                    data.PreloadCount = StaticReader.ReadInt(GetContent(col++));

                    if (!typeDic.ContainsKey(data.MazeKid))
                    {
                        typeDic.Add(data.MazeKid, new List<ResourceData>());
                    }
                    typeDic[data.MazeKid].Add(data);

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

