using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using StaticData;
using Base;

namespace GameLogic
{
	public class Block : Entity
	{
		public new BlockData Data
		{
			get { return data as BlockData; }
			protected set { data = value; }
		}
		
		public new BlockScript Script
		{
			get { return script as BlockScript; }
			protected set { script = value; }
		}

		public int Col;
		public int Row;
		public int Direction;

        public int ExplorationKid;
        public int RandomID;

		public bool IsRoom { get { return Data.Cols > 1 || Data.Rows > 1;} }

		public bool Contains(int col, int row)
		{
			if (Data.BlockType == BlockType.Passage)
			{
				return Col == col && Row == row;
			}
			else
			{
                MazePosition basePos = MazeUtil.GetRoomBasePos(Direction, Col, Row, Data.Cols, Data.Rows, Data.LeftOffset);
                return col >= basePos.Col && row >= basePos.Row && col <= basePos.Col + Data.Cols - 1 && row <= basePos.Row + Data.Rows - 1;
			}
		}

		public void SetPosition(int col, int row)
		{
			Col = col;
			Row = row;
			MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
			Vector3 worldPosition = MazeUtil.GetWorldPosition(col, row, mazeData.BlockSize);
			SetPosition(worldPosition);
		}

		public void SetRotation(int direction)
		{
			Direction = direction;
			SetRotation(direction * 90f);
		}

		public void InitRandomDecorations()
		{
			Script.InitRandomDecorations();
		}

        public void ForeachNode(Action<MazePosition> func)
        {
            if(IsRoom)
            {
                MazePosition basePos = MazeUtil.GetRoomBasePos(Direction, Col, Row, Data.Cols, Data.Rows, Data.LeftOffset);
                for(int i = basePos.Col; i < basePos.Col + Data.Cols; ++i)
                {
                    for(int j = basePos.Row; j < basePos.Row + Data.Rows; ++j)
                    {
                        MazePosition pos = new MazePosition(i, j);
                        func(pos);
                    }
                }
            }
            else
            {
                MazePosition pos = new MazePosition(Col, Row);
                func(pos);
            }
        }

		public static Block Create(MazeNode node)
		{
			if (node == null)
			{
				BaseLogger.Log("null node");
				return null;
			}

			ResourceManager resManager = ResourceManager.Instance;
			
			Block block = new Block();
			block.Uid = Guid.NewGuid().ToString();
            block.Data = node.Data;
			block.Script = resManager.LoadAsset<BlockScript>(ObjectType.GameObject, block.Data.GetResPath());
            block.Script.Uid = block.Uid;
            block.Script.transform.parent = RootTransform.Instance.BlockRoot;

			if (node is MazeRoom)
			{
				MazeRoom room = node as MazeRoom;
				block.SetRotation(room.Direction);
				block.SetPosition(room.Col, room.Row);
			}
			else
			{
				block.SetPosition(node.Col, node.Row);
                block.SetRotation(node.Direction);
				block.InitRandomDecorations();
			}

            block.ExplorationKid = node.ExplorationKid;
            block.RandomID = Maze.Instance.Data.Kid * 10000 + Block.GetBlockKey(block.Col, block.Row);

			return block;
		}

		public static void Recycle(Block block)
		{
			if (block != null)
			{
				block.Data = null;
				block.Script.Reset();
				ResourceManager.Instance.RecycleAsset(block.Script.gameObject);
				block.Script = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null block!");
			}
		}

		public static int GetBlockKey(int col, int row)
		{
			return col * 100 + row;
		}
	}
}

