using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
	public class BlockProxy : Proxy
	{
		public MazeNode StartNode { get; private set; }

		public List<MazeNode> ToDeleteNodeList;
		public List<MazeNode> ToCreateNodeList;

		private List<MazeNode> prevAroundList;

		private MazeData mazeData;
		private MazeTable mazeTable;

		private Dictionary<int, Block> blockDic;

		private List<MazeNode> mockNodeList;

		public BlockProxy() : base()
		{
			blockDic = new Dictionary<int, Block>();
			mockNodeList = new List<MazeNode>();
		}

		public void Init()
		{
			mazeData = MazeDataManager.Instance.CurrentMazeData;
			MazeBuilder builder = new MazeBuilder(mazeData);
			mazeTable = builder.Build();
			StartNode = mazeTable.GetNode(mazeData.StartCol, mazeData.StartRow);

            InitGlobalExplorationPositions();
            TestGlobalExplorationPositions();

			ToDeleteNodeList = new List<MazeNode>();
			ToCreateNodeList = new List<MazeNode>();
			prevAroundList = new List<MazeNode>();
		}

		public void Dispose()
		{
			blockDic.Clear();
			mockNodeList.Clear();

			ToDeleteNodeList.Clear();
			ToCreateNodeList.Clear();
			prevAroundList.Clear();
			mazeTable = null;
			StartNode = null;
			mazeData = null;
		}

		#region Creating and Remove Blocks

		public void UpdateAroundMazeNode(int col, int row, int scope)
		{
			List<MazeNode> aroundList = mazeTable.GetAroundNode(col, row, scope);

			ToDeleteNodeList.Clear();
			ToCreateNodeList.Clear();

			for (int i = 0; i < prevAroundList.Count; ++i)
			{
				MazeNode node = prevAroundList[i];
				if (aroundList.IndexOf(node) == -1)
				{
					ToDeleteNodeList.Add(node);
				}
			}
			for (int i = 0; i < aroundList.Count; ++i)
			{
				MazeNode node = aroundList[i];
				if (prevAroundList.IndexOf(node) == -1)
				{
					ToCreateNodeList.Add(node);
				}
			}

			prevAroundList = aroundList;
		}

		public Block AddBlock(MazeNode node)
		{
			if (node is MazeRoom)
			{
				MazeRoom room = node as MazeRoom;
				if (room.HasCreated)
				{
					return null;
				}
				else
				{
					room.HasCreated = true;
				}
			}

			Block block = Block.Create(node);

			int key = Block.GetBlockKey(node.Col, node.Row);
			if (blockDic.ContainsKey(key))
			{
				BaseLogger.LogFormat("Add same block: {0}", key.ToString());
			}
			blockDic.Add(key, block);
			node.AboveBlock = block;

			return block;
		}
		public void RemoveBlock(MazeNode node)
		{
			if (node is MazeRoom)
			{
				MazeRoom room = node as MazeRoom;
				//Here HasCreated means "HasDeleted" in fact...
				if (room.HasCreated)
				{
					room.HasCreated = false;
				}
				else
				{
					return;
				}
			}

			int key = Block.GetBlockKey(node.Col, node.Row);
			if (!blockDic.ContainsKey(key))
			{
				BaseLogger.LogFormat("Remove none-exist block: {0}", key.ToString());
			}
			Block block = blockDic[key];
			blockDic.Remove(key);
			node.AboveBlock = null;

			Block.Recycle(block);
		}

		//Room??
		public Block GetBlockAtPosition(int col, int row)
		{
			if (mazeTable.CheckOccupied(col, row))
			{
				MazeNode node = GetNode(col, row);
				return node.AboveBlock;
			}
			else
			{
				return null;
			}
		}
		public MazeNode GetNode(int col, int row)
		{
			return mazeTable.GetNode(col, row);
		}

		#endregion

		#region Block Path Finding

		public void UpdateSearchIndex(int col, int row)
		{
			int scope = GlobalConfig.BlockConfig.RefreshScope;
			mazeTable.IterateNodesInScope(col, row, scope, (object param) =>
        	{
				MazeNode node = param as MazeNode;
				node.SearchIndex = -1;
			});
			MazeNode startNode = mazeTable.GetNode(col, row);
			UpdateSearchIndexRecurrsively(startNode, startNode, 0);
		}
		private void UpdateSearchIndexRecurrsively(MazeNode startNode, MazeNode searchNode, int distance)
		{
			int scope = GlobalConfig.BlockConfig.RefreshScope;
			if (Mathf.Abs(searchNode.Col - startNode.Col) > scope || Mathf.Abs(searchNode.Row - startNode.Row) > scope )
			{
				return;
			}
			
			searchNode.SearchIndex = distance++;
//			Logger.Log(searchNode.Col, searchNode.Row, searchNode.SearchIndex);
			for (int i = 0; i < searchNode.LinkList.Length; ++i)
			{
				MazeNode nextNode = searchNode.LinkList[i];
				if (nextNode != null && nextNode.SearchIndex == -1)
				{
					UpdateSearchIndexRecurrsively(startNode, nextNode, distance);
				}
			}
		}

		public MazeNode FindNextSearchNode(int col, int row)
		{
			MazeNode startNode = mazeTable.GetNode(col, row);
			if (startNode.SearchIndex == 0)
			{
				return startNode;
			}

			MazeNode nextNode = null;
			for (int i = 0; i < startNode.LinkList.Length; ++i)
			{
				MazeNode node = startNode.LinkList[i];
				if (node != null && node.SearchIndex >= 0)
				{
					if (nextNode == null || node.SearchIndex < nextNode.SearchIndex)
					{
						nextNode = node;
					}
				}
			}
			return nextNode;
		}

		#endregion
		
		#region Mock Block

		public List<MazeNode> MockNodeList
		{
			get
			{
				int count = mockNodeList.Count;
				for (int i = 0; i < count; ++i)
				{
					MazeRoom room = mockNodeList[i] as MazeRoom;
					if (room != null)
					{
						room.HasCreated = false;
					}
				}
				return mockNodeList;
			}
		}

		public void AddMockNode(int col, int row)
		{
			MazeNode node = mazeTable.GetNode(col, row);
			if (!node.HasExplored)
			{
				mockNodeList.Add(node);
				node.HasExplored = true;
			}
		}

		#endregion

        #region Global Exploration Position Init

        public void InitGlobalExplorationPositions()
        {
            Debug.Log(mazeTable.NodeCount);
            foreach(ExplorationType type in mazeData.GlobalExplorationCountDic.Keys)
            {
                int globalExplorationCount = mazeData.GlobalExplorationCountDic[type];
                List<int> indexList = Utils.GetRandomIndexList(mazeTable.NodeCount, globalExplorationCount);
                for (int i = 0; i < indexList.Count; ++i)
                {
                    int index = indexList[i];
                    MazeNode node = mazeTable.GetNode(index);
                    node.ExplorationType = type;
                }
            }
        }

        public void TestGlobalExplorationPositions()
        {
            mazeTable.ForeachMazeNode((MazeNode node) =>
                {
                    if (node.ExplorationType != ExplorationType.Common)
                    {
                        Debug.LogFormat("{0}, col={1}, row={2}", node.ExplorationType, node.Col, node.Row);
                    }
                });
        }

        #endregion
	}
}