using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using StaticData;
using Base;

namespace GameLogic
{
    public class Maze
    {
		public MazeData Data;

		private long startTicks;

		private static Maze instance;
		public static Maze Instance
		{
			get
			{
				if (instance == null) instance = new Maze();
				return instance;
			}
		}

		public void Init()
		{
			startTicks = DateTime.Now.Ticks;
			Data = MazeDataManager.Instance.CurrentMazeData;
            if(RandomUtils.Seed == 0)
            {
                RandomUtils.Seed = UnityEngine.Random.Range(0, 20140413);
            }
		}

		public float ElapsedTime
		{
			get
			{
				return (DateTime.Now.Ticks - startTicks) / 10000000;
			}
		}

		public Vector2 GetMazePosition(Vector3 position)
		{
			float blockSize = Data.BlockSize;
			int col, row;
			MazeUtil.GetMazePosition(position, blockSize, out col, out row);
			return new Vector2(col, row);
		}

    }
}

