using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class SkillEffectDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, SkillEffectData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, SkillEffectData>();

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    SkillEffectData data = new SkillEffectData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Ratio = StaticReader.ReadFloat(GetContent(col++));
                    data.AreaKid = StaticReader.ReadInt(GetContent(col++));
                    data.BulletKid = StaticReader.ReadInt(GetContent(col++));
                    data.BuffKidList = StaticReader.ReadIntList(GetContent(col++));

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

