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
            RandomUtils.Seed = 13880388;
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

        public const int BLOCK_MARK = 1;
        public const int HALL_MARK = 2;
        //XXYZZZZ...X:MazeKid, Y:Mark, ZZZZ:block id or hall id
        public static int GetLocation(int mazeKid, int hallKid)
        {
            return mazeKid * 100000 + HALL_MARK * 10000 + hallKid;
        }
        public static int GetLocation(int mazeKid, int col, int row)
        {
            return mazeKid * 100000 + BLOCK_MARK * 10000 + col * 100 + row;
        }
        public static int GetLocation(int mazeKid, Vector3 worldPosition)
        {
            if(Hall.Instance != null)
            {
                return GetLocation(mazeKid, Hall.Instance.Data.Kid);
            }
            else
            {
                Vector2 mazePos = Maze.Instance.GetMazePosition(worldPosition);
                return GetLocation(mazeKid, (int)mazePos.x, (int)mazePos.y);
            }
        }
    }
}

