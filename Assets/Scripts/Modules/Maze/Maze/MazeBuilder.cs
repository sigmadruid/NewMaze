using UnityEngine;

using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public enum LinkType
	{
		Average,
		ByPossibilty,
	}

    public class MazeBuilder
    {
		private List<MazeNode> nodeList = new List<MazeNode>();

		private MazeData mazeData;
		private MazeTable mazeTable;

		public MazeBuilder(MazeData data)
        {
			this.mazeData = data;
        }

		public MazeTable Build()
		{
			int seed = Random.Range(0, 10000);
//			Random.seed = seed;
//            BaseLogger.LogFormat("Maze seed is: {0}", seed);

            //For test
//            Random.seed = 7928;
			
			mazeTable = new MazeTable();
			mazeTable.Init(mazeData.Cols, mazeData.Rows);

			nodeList.Clear();
			
			MazeNode firstNode = BuildFirstBlock();
			mazeTable.SetNode(firstNode, firstNode.Col, firstNode.Row);

			nodeList.Add(firstNode);
			mazeTable.AddMazeNode(firstNode);
			while(nodeList.Count > 0)
			{
//				Debug.Log((node.Col * 3).ToString() + ", " + (node.Row * 3).ToString() + "  " + node.Up.ToString() + ", " + node.Right.ToString() + ", " + node.Down.ToString() + ", " + node.Left.ToString());
				ExpandNode(nodeList[0]);
			}

			return mazeTable;
		}

		private MazeNode BuildFirstBlock()
		{
			int col = mazeData.StartCol;
			int row = mazeData.StartRow;
			MazeNode node = CreateMazeNode(col, row);
			return node;
		}

		private MazeNode CreateMazeNode(int col, int row)
		{
			MazeNode node = new MazeNode();
			node.Col = col;
			node.Row = row;
			return node;
		}
		private MazeRoom CreateMazeRoom(BlockData data, int direction, int col, int row)
		{
			MazeRoom room = new MazeRoom();
			room.Data = data;
			room.Direction = direction;
			room.Col = col;
			room.Row = row;
			return room;
		}

		private void ExpandNode(MazeNode node)
		{
			int col, row;
			int baseCol, baseRow;

			nodeList.Remove(node);

			for (int direction = 0; direction < 4; ++direction)
			{
				if (node.LinkList[direction] != null)
				{
					continue;
				}

				if (RandomUtils.Value() < mazeData.LinkRate)
				{
					int oppositeDirection = MazeUtil.GetOppositeDirection(direction);
					MazeUtil.GetNeighbor(direction, node.Col, node.Row, out col, out row);
					if (mazeTable.CheckRange(col, row))
					{
						if (RandomUtils.Value() < mazeData.PassageRate)
						{
							//Node
							if (!mazeTable.CheckOccupied(col, row))
							{
								MazeNode newNode = CreateMazeNode(col, row);
								node.LinkList[direction] = newNode;
								newNode.LinkList[oppositeDirection] = node;
								nodeList.Add(newNode);
								mazeTable.SetNode(newNode, newNode.Col, newNode.Row);
								mazeTable.AddMazeNode(newNode);
							}
							else
							{
								node.LinkList[direction] = null;
							}
						}
						else
						{
							//Room
							BlockData roomData = BlockDataManager.Instance.GetRandomRoomData();

							MazeUtil.GetRoomBasePos(direction, col, row, roomData.Cols, roomData.Rows, roomData.LeftOffset, out baseCol, out baseRow);
							if (mazeTable.CheckRoom(baseCol, baseRow, roomData.Cols, roomData.Rows))
						    {
								MazeRoom room = CreateMazeRoom(roomData, direction, col, row);
								for (int i = baseCol; i < baseCol + roomData.Cols; ++i)
								{
									for (int j = baseRow; j < baseRow + roomData.Rows; ++j)
									{
										mazeTable.SetNode(room, i, j);
									}
								}
								node.LinkList[direction] = room;
								room.LinkList[oppositeDirection] = node;
								mazeTable.AddMazeNode(room);
							}
							else
							{
								node.LinkList[direction] = null;
							}
						}
					}
					else
					{
						node.LinkList[direction] = null;
					}
				}
			}

		}

    }
}

