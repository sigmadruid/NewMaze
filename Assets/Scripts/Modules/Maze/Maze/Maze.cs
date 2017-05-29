using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using StaticData;
using Base;

namespace GameLogic
{
    public struct MazePosition
    {
        public int Col;
        public int Row;
        public MazePosition(int col, int row)
        {
            Col = col;
            Row = row;
        }
    }
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
            Main main = GameObject.FindObjectOfType<Main>();
            int seed;
            if(main == null)
            {
                MazeMain mazeMain = GameObject.FindObjectOfType<MazeMain>();
                seed = mazeMain.Seed;
            }
            else
            {
                seed = main.Seed;
            }
            if(seed != 0)
            {
                Seed = main.Seed;
            }
            else if(Seed == 0)
            {
                Seed = UnityEngine.Random.Range(0, 20140413);
            }
            RandomUtils.Seed = Seed;
            Debug.Log("random seed: " + Seed.ToString());
		}

        public int Seed { get; set; }
		public float ElapsedTime
		{
			get
			{
				return (DateTime.Now.Ticks - startTicks) / 10000000;
			}
		}

        public MazePosition GetMazePosition(Vector3 position)
		{
			float blockSize = Data.BlockSize;
            MazePosition mazePos = MazeUtil.GetMazePosition(position, blockSize);
            return mazePos;
		}

        public const int BLOCK_MARK = 1;
        public const int HALL_MARK = 2;
        //XXYZZZZ...X:MazeKid, Y:Mark, ZZZZ:block id or hall id
        public static int GetLocation(int mazeKid, int hallKid)
        {
            return IDManager.Instance.GetID(mazeKid) * 100000 + HALL_MARK * 10000 + hallKid;
        }
        public static int GetLocation(int mazeKid, int col, int row)
        {
            return IDManager.Instance.GetID(mazeKid) * 100000 + BLOCK_MARK * 10000 + col * 100 + row;
        }
        public static int GetLocation(int mazeKid, Vector3 worldPosition)
        {
            if(Hall.Instance != null)
            {
                return GetLocation(mazeKid, Hall.Instance.Data.Kid);
            }
            else
            {
                MazePosition mazePos = Maze.Instance.GetMazePosition(worldPosition);
                return GetLocation(mazeKid, mazePos.Col, mazePos.Row);
            }
        }
        public static int GetCurrentLocation(int hallKid)
        {
            int mazeKid = Maze.Instance.Data.Kid;
            return GetLocation(mazeKid, hallKid);
        }
        public static int GetCurrentLocation(int col, int row)
        {
            int mazeKid = Maze.Instance.Data.Kid;
            return GetLocation(mazeKid, col, row);
        }
        public static int GetCurrentLocation(Vector3 worldPosition)
        {
            int mazeKid = Maze.Instance.Data.Kid;
            return GetLocation(mazeKid, worldPosition);
        }
    }
}

