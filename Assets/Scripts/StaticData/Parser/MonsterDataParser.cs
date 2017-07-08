using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class MonsterDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, MonsterData> kvDic, out List<MonsterData> dataList)
        {
            LoadFile(CONFIG_PATH + name);
			
			kvDic = new Dictionary<int, MonsterData>();
			dataList = new List<MonsterData>();
			
            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;

                    MonsterData data = new MonsterData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Res2D = StaticReader.ReadString(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.HP = StaticReader.ReadInt(GetContent(col++));
                    data.SP = StaticReader.ReadInt(GetContent(col++));
                    data.Attack = StaticReader.ReadInt(GetContent(col++));
                    data.Defense = StaticReader.ReadInt(GetContent(col++));
                    data.Critical = StaticReader.ReadInt(GetContent(col++));
                    data.Dodge = StaticReader.ReadInt(GetContent(col++));
                    data.MoveSpeed = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackSpeed = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackType = StaticReader.ReadEnum<AttackType>(GetContent(col++));
                    data.SkillList = StaticReader.ReadIntList(GetContent(col++));

                    data.Exp = StaticReader.ReadInt(GetContent(col++));
                    data.Size = StaticReader.ReadEnum<MonsterSize>(GetContent(col++));
                    data.DetectRange = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackDelay = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackRange = StaticReader.ReadFloat(GetContent(col++));
                    data.DodgeDelay = StaticReader.ReadFloat(GetContent(col++));
                    data.DodgeRange = StaticReader.ReadFloat(GetContent(col++));
                    data.AppearWeight = StaticReader.ReadInt(GetContent(col++));
                    data.DropKid = StaticReader.ReadInt(GetContent(col++));

                    kvDic.Add(data.Kid, data);
                    dataList.Add(data);

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

