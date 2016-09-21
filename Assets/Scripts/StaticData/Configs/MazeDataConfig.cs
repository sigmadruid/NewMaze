using UnityEngine;
using System;
using System.Collections.Generic;

namespace StaticData
{
	public class MazeDataConfig
	{
		public Dictionary<int, MazeData> MazeDataDic;

		public MazeDataConfig ()
		{
			MazeDataDic = new Dictionary<int, MazeData>();
			MazeData data;

			data = new MazeData();
			data.Kid = 1;
			data.Cols = 30;
			data.Rows = 30;
			data.StartCol = 15;
			data.StartRow = 15;
			data.LinkRate = 0.5f;
			data.PassageRate = 0.9f;
			data.BlockSize = 10f;

			data.MonsterMaxCount = 2;
			data.NPCMaxCount = 2;
			data.ExplorationMaxCount = 3;
			MazeDataDic.Add(data.Kid, data);

		}
	}
}

