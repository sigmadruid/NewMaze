using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class MazeNode
	{
		public int Col;

		public int Row;

        /// <summary>
        /// Link to other nodes. The sequence: Up, Right, Down, Left
        /// </summary>
        public MazeNode[] LinkList = new MazeNode[4]{null, null, null, null};

		/// <summary>
		/// The index is for monster path finding. Monster will always walk to the lower index.
		/// </summary>
		public int SearchIndex = -1;

        /// <summary>
        /// The block corresponds to this node.
        /// </summary>
		public Block AboveBlock;

        /// <summary>
        /// The type of the exploration in this block.
        /// </summary>
        public ExplorationType ExplorationType = ExplorationType.Common;

		public string Format()
		{
			return string.Format("(col:{0}, row:{1}, searchIndex:{2})", Col, Row, SearchIndex);
		}

	}

	public class MazeRoom : MazeNode
	{
        /// <summary>
        /// A digit to indicate whether the room has created/deleted.
        /// </summary>
		public bool HasCreated;

        /// <summary>
        /// The room's entry direction.
        /// </summary>
		public int Direction;

		public BlockData Data;
	}

    public class MazeTable
    {
		public int Cols { get; private set; }
		public int Rows { get; private set; }

		private List<MazeNode> mazeNodesList = new List<MazeNode>();
		private MazeNode[,] allNodes;

		public void Init(int cols, int rows)
		{
			Cols = cols;
			Rows = rows;
			allNodes = new MazeNode[cols, rows];
		}

        public int NodeCount
        {
            get { return mazeNodesList.Count; }
        }

		public MazeNode GetNode(int col, int row)
		{
			if (!CheckRange(col, row))
			{
				BaseLogger.LogFormat("Maze node out of range: {0}, {1}", col, row);
			}
			return allNodes[col, row];
		}
        public MazeNode GetNode(int index)
        {
            if (index < 0 || index >= mazeNodesList.Count)
            {
                BaseLogger.LogFormat("Maze node out of range: {0}", index);
            }
            return mazeNodesList[index];
        }
		public void SetNode(MazeNode node, int col, int row)
		{
			if (!CheckRange(col, row))
			{
				BaseLogger.LogFormat("Maze node out of range: {0}, {1}", node.Col, node.Row);
			}
			allNodes[col, row] = node;
		}

		public bool CheckRange(int col, int row)
		{
			return col >= 0 && row >= 0 && col < Cols && row < Rows;
		}
		public bool CheckOccupied(int col, int row)
		{
			MazeNode node = GetNode(col, row);
			return node != null;
		}
		public bool CheckRoom(int baseCol, int baseRow, int cols, int rows)
		{
			if (!CheckRange(baseCol, baseRow))
			{
				return false;
			}
			if (!CheckRange(baseCol + cols - 1, baseRow + rows - 1))
			{
				return false;
			}
			for (int i = baseCol; i < baseCol + cols; ++i)
			{
				for (int j = baseRow; j < baseRow + rows; ++j)
				{
//					BaseLogger.Log(i * 3, j * 3, GetNode(i, j));
					if (CheckOccupied(i, j))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool CheckInScope(int centerCol, int centerRow, int scope, int col, int row)
		{
			int imin = Mathf.Max(0, centerCol - scope);
			int jmin = Mathf.Max(0, centerRow - scope);
			int imax = Mathf.Max(Cols - 1, centerCol + scope);
			int jmax = Mathf.Max(Rows - 1, centerRow + scope);
			return col >= imin && col <= imax && row >= jmin && row <= jmax;
		}

		public List<MazeNode> GetAroundNode(int col, int row, int scope)
		{
			if (!CheckRange(col, row) || scope < 0)
			{
				return null;
			}

			int imin = Mathf.Max(col - scope, 0);
			int jmin = Mathf.Max(row - scope, 0);
			int imax = Mathf.Min(col + scope, Cols - 1);
			int jmax = Mathf.Min(row + scope, Rows - 1);

			List<MazeNode> list = new List<MazeNode>();
			for (int i = imin; i <= imax; ++i)
			{
				for (int j = jmin; j <= jmax; ++j)
				{
//					if (i == col && j == row)
//					{
//						continue;
//					}

					MazeNode node = GetNode(i, j);
					if (node != null)
					{
						list.Add(node);
					}
				}
			}
			return list;
		}
        public List<MazeNode> GetAllNodes()
        {
            return new List<MazeNode>(mazeNodesList);
        }

        public void IterateNodesInScope(int centerCol, int centerRow, int scope, System.Action<MazeNode> callback)
		{
			int imin = Mathf.Max(0, centerCol - scope);
			int jmin = Mathf.Max(0, centerRow - scope);
			int imax = Mathf.Min(Cols - 1, centerCol + scope);
			int jmax = Mathf.Min(Rows - 1, centerRow + scope);
			for (int i = imin; i <= imax; ++i)
			{
				for (int j = jmin; j <= jmax; ++j)
				{
					MazeNode node = GetNode(i, j);
					if (node != null)
					{
						callback(node);
					}
				}
			}
		}

		public void AddMazeNode(MazeNode node)
		{
			mazeNodesList.Add(node);
		}
		public void ForeachMazeNode(Action<MazeNode> callback)
		{
			for (int i = 0; i < mazeNodesList.Count; ++i)
			{
				callback(mazeNodesList[i]);
			}
		}
    }
}

