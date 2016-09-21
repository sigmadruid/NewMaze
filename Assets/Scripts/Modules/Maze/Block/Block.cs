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

        public ExplorationType ExplorationType;

		public bool IsRoom { get { return Data.Cols > 1 || Data.Rows > 1;} }

		public bool Contains(int col, int row)
		{
			if (Data.BlockType == BlockType.Passage)
			{
				return Col == col && Row == row;
			}
			else
			{
				int baseCol, baseRow;
				MazeUtil.GetRoomBasePos(Direction, Col, Row, Data.Cols, Data.Rows, Data.LeftOffset, out baseCol, out baseRow);
				return col >= baseCol && row >= baseRow && col <= baseCol + Data.Cols - 1 && row <= baseRow + Data.Rows - 1;
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

			PassageType passageType = MazeUtil.GetPassageType(node);
			if (node is MazeRoom)
			{
				MazeRoom room = node as MazeRoom;
				block.Data = room.Data;
			}
			else
			{
				block.Data = BlockDataManager.Instance.GetRandomPassageData(passageType); 
			}

			block.Script = resManager.LoadAsset<BlockScript>(ObjectType.GameObject, block.Data.GetResPath());
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
				int direction = MazeUtil.GetPassageDirection(node, passageType);
				block.SetRotation(direction);
				block.InitRandomDecorations();
			}

            block.ExplorationType = node.ExplorationType;

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

