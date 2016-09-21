using UnityEngine;

using System;

using StaticData;

namespace GameLogic
{
    public static class MazeUtil
    {
		public static void GetMazePosition(Vector3 position, float blockSize, out int outCol, out int outRow)
		{
			outCol = (int)(position.x / blockSize + 0.5f);
			outRow = (int)(position.z / blockSize + 0.5f);
		}
		public static Vector3 GetWorldPosition(int col, int row, float blockSize)
		{
			return new Vector3(col * blockSize, 0, row * blockSize);
		}

		public static int GetOppositeDirection(int direction)
		{
			switch(direction)
			{
				case 0:
					return 2;
				case 1:
					return 3;
				case 2:
					return 0;
				case 3:
					return 1;
			}
			return 0;
		}
		public static int GetNextDirection(int direction)
		{
			switch(direction)
			{
				case 0:
					return 1;
				case 1:
					return 2;
				case 2:
					return 3;
				case 3:
					return 0;
			}
			return 0;
		}

		public static void GetNeighbor(int direction, int inCol, int inRow, out int outCol, out int outRow)
		{
			switch(direction)
			{
				case 0:
					outCol = inCol;
					outRow = inRow + 1;
					return;
				case 1:
					outCol = inCol + 1;
					outRow = inRow;
					return;
				case 2:
					outCol = inCol;
					outRow = inRow - 1;
					return;
				case 3:
					outCol = inCol - 1;
					outRow = inRow;
					return;
			}
			outCol = inCol;
			outRow = inRow;
		}

		//Base pos is the bottom-left point of the room.
		public static void GetRoomBasePos(int direction, int inCol, int inRow, int cols, int rows, int leftOffset, out int outCol, out int outRow)
		{
			switch(direction)
			{
				case 0:
					outCol = inCol - leftOffset + 1;
					outRow = inRow;
					return;
				case 1:
					outCol = inCol;
					outRow = inRow - leftOffset + 1;
					return;
				case 2:
					outCol = inCol - leftOffset + 1;
					outRow = inRow - rows + 1;
					return;
				case 3:
					outCol = inCol - cols + 1;
					outRow = inRow - leftOffset + 1;
					return;
			}
			outCol = inCol;
			outRow = inRow;

		}

		public static int GetNodeLinkCount(MazeNode node)
		{
			int sum = 0;
			for (int i = 0; i < 4; ++i)
			{
				sum += node.LinkList[i] != null ? 1 : 0;
			}
			return sum;
		}

		public static PassageType GetPassageType(MazeNode node)
		{
			int linkCount = 0;
			for (int i = 0; i < 4; ++i)
			{
				linkCount += node.LinkList[i] != null ? 1 : 0;
			}
			if (linkCount == 1)
			{
				return PassageType.One;
			}
			else if (linkCount == 2)
			{
				if (node.LinkList[0] == null && node.LinkList[2] == null ||
				    node.LinkList[1] == null && node.LinkList[3] == null)
				{
					return PassageType.TwoLine;
				}
				else
				{
					return PassageType.TwoTurn;
				}
			}
			else if (linkCount == 3)
			{
				return PassageType.Three;
			}
			else
			{
				return PassageType.Four;
			}
		}

		public static int GetPassageDirection(MazeNode node, PassageType type)
		{
			if (type == PassageType.One)
			{
				for (int i = 0; i < 4; ++i)
				{
					if (node.LinkList[i] != null)
					{
						return GetOppositeDirection(i);
					}
				}
			}
			else if (type == PassageType.TwoLine)
			{
				for (int i = 0; i < 4; ++i)
				{
					if (node.LinkList[i] != null)
					{
						return i;
					}
				}
			}
			else if (type == PassageType.TwoTurn)
			{
				for (int i = 0; i < 4; ++i)
				{
					if (node.LinkList[i] == null && node.LinkList[GetNextDirection(i)] != null)
					{
						return i;
					}
				}
			}
			else if (type == PassageType.Three)
			{
				for (int i = 0; i < 4; ++i)
				{
					if (node.LinkList[i] == null)
					{
						return i;
					}
				}
			}
			return 0;
		}

    }
}

