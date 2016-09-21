using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
	public class MazeDataManager
	{
		private Dictionary<int, MazeData> kvDic;
		private MazeData currentMaze;

		private static MazeDataManager instance;
		public static MazeDataManager Instance
		{
			get
			{
				if (instance == null) instance = new MazeDataManager();
				return instance;
			}
		}

		public void Init ()
		{
			MazeDataParser parser = new MazeDataParser();
			parser.Parse("MazeDataConfig.csv", out kvDic);
		}

		public MazeData CurrentMazeData
		{
			get
			{
				if (currentMaze == null)
				{
					int id = IDManager.Instance.GetID(IDType.Maze, 1);
					currentMaze = kvDic[id];
				}
				return currentMaze;
			}
		}

	}
}

