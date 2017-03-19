using System;
using System.Collections.Generic;

namespace StaticData
{
    public class MonsterDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, MonsterData> kvDic, out List<MonsterData> dataList)
        {
            LoadFile(CONFIG_PATH + name);
			
			kvDic = new Dictionary<int, MonsterData>();
			dataList = new List<MonsterData>();
			
			while(!EndOfRow)
			{
				int col = 0;

				MonsterData data = new MonsterData();
				data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
                data.Res2D = ReadString(col++);
				data.Res3D = ReadString(col++);
				data.HP = ReadInt(col++);
				data.Attack = ReadInt(col++);
				data.Defense = ReadInt(col++);
				data.Critical = ReadInt(col++);
				data.Dodge = ReadInt(col++);
                data.MoveSpeed = ReadFloat(col++);
                data.AttackSpeed = ReadFloat(col++);
				data.AttackType = ReadEnum<AttackType>(col++);
                data.SkillList = ReadIntList(col++);
                data.DetectRange = ReadFloat(col++);
				data.AttackDelay = ReadFloat(col++);
				data.AttackRange = ReadFloat(col++);
				data.DodgeDelay = ReadFloat(col++);
				data.DodgeRange = ReadFloat(col++);
				data.AppearWeight = ReadInt(col++);
                data.DropKid = ReadInt(col++);

				kvDic.Add(data.Kid, data);
				dataList.Add(data);
				
				NextLine();
			}
        }
    }
}

