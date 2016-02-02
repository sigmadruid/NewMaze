using System;
using System.Collections.Generic;

namespace GameLogic
{
	public class BuffInfo
	{
		private Dictionary<BattleAttribute, int> attributeDic;

		public BuffInfo ()
		{
			attributeDic = new Dictionary<BattleAttribute, int>();
		}
	}
}

