using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Base;

namespace GameLogic
{
    public class PlayerProxy : Proxy
    {
		private PlayerInfo currentInfo;

		private Dictionary<string, PlayerInfo> playerInfoDic = new Dictionary<string, PlayerInfo>();

		public void Init(int currentIndex, List<PlayerRecord> recordList)
		{
			for (int i = 0; i < recordList.Count; ++i)
			{
				PlayerInfo info = new PlayerInfo(recordList[i]);
				playerInfoDic.Add(info.Uid, info);

				if (i == currentIndex)
				{
					currentInfo = info;
				}
			}
		}

		public void ChangeGold(int gold)
		{
		}

		public void AddExp(int exp)
		{
		}

		public int MyLevel { get { return currentInfo.Level; }}
		public int MyExp { get { return currentInfo.Exp; }}
		public int MyGold { get { return currentInfo.Gold; }}
		
    }
}

