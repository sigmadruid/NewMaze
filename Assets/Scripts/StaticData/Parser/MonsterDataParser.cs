using System;
using System.Collections.Generic;

namespace StaticData
{
    public class MonsterDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, MonsterData> kvDic, out List<MonsterData> dataList)
        {
			LoadFile(name);
			
			kvDic = new Dictionary<int, MonsterData>();
			dataList = new List<MonsterData>();
			
			while(!EndOfRow)
			{
				int col = 0;

				MonsterData data = new MonsterData();
				data.Kid = ReadInt(col++);
				data.Name = ReadInt(col++);
				data.Res3D = ReadString(col++);
				data.HP = ReadInt(col++);
				data.Attack = ReadInt(col++);
				data.Defense = ReadInt(col++);
				data.Critical = ReadInt(col++);
				data.Dodge = ReadInt(col++);
				data.MoveSpeed = ReadFloat(col++);
				data.DetectRange = ReadFloat(col++);
				data.AttackType = ReadEnum<AttackType>(col++);
				data.AttackDelay = ReadFloat(col++);
				data.AttackRange = ReadFloat(col++);
				data.DodgeDelay = ReadFloat(col++);
				data.DodgeRange = ReadFloat(col++);
				data.BulletKid = ReadInt(col++);
				data.AppearWeight = ReadInt(col++);
				data.DropKidList = ReadIntList(col++);

				kvDic.Add(data.Kid, data);
				dataList.Add(data);
				
				NextLine();
			}
        }
    }
}

