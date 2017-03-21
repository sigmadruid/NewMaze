using System;
using System.Collections.Generic;

namespace StaticData
{
    public class PlotDataParser : BaseParser
    {
        public void Parse(string name, out List<SegmentData> dataList)
        {
            LoadFile(PLOT_PATH + name);

            dataList = new List<SegmentData>();
            int step = 1;

            while(!EndOfRow)
            {
                int col = 0;
                SegmentData data = new SegmentData();

                data.Step = step++;
                data.StartTime = StaticReader.ReadFloat(GetContent(col++));
                data.EndTime = StaticReader.ReadFloat(GetContent(col++));
                data.Type = StaticReader.ReadEnum<SegmentType>(GetContent(col++));
                data.Target = StaticReader.ReadString(GetContent(col++));
                data.Position = StaticReader.ReadVector3(GetContent(col++));
                data.Orientation = StaticReader.ReadVector3(GetContent(col++));
                data.Param1 = StaticReader.ReadString(GetContent(col++));
                data.Param2 = StaticReader.ReadString(GetContent(col++));
                data.Param3 = StaticReader.ReadString(GetContent(col++));

                dataList.Add(data);
                NextLine();
            }
        }
    }
}

