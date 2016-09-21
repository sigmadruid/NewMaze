using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class PlayerInfo
    {
		public string Uid;

		public string Name;

		public int Level;

		public int Exp;

		public int Gold;

		public List<int> EmbattleHeroIDList = new List<int>();

		public PlayerInfo(PlayerRecord record)
		{
			this.Uid = record.Uid;
			this.Name = record.Name;
			this.Level = record.Level;
			this.Exp = record.Exp;
			this.Gold = record.Gold;
			this.Gold = record.Gold;
		}

    }
}

