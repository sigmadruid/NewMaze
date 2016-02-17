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
        public delegate void IterateFunc(Block block);

		public MazeNode StartNode { get; private set; }

        public List<MazeNode> ToDeleteNodeList = new List<MazeNode>();
        public List<MazeNode> ToCreateNodeList = new List<MazeNode>();

        private List<MazeNode> prevAroundList = new List<MazeNode>();

		private MazeData mazeData;
		private MazeTable mazeTable;

        private Dictionary<int, Block> blockDic = new Dictionary<int, Block>();

		public void Init()
		{
			mazeData = MazeDataManager.Instance.CurrentMazeData;
			MazeBuilder builder = new MazeBuilder(mazeData);
			mazeTable = builder.Build();
			StartNode = mazeTable.GetNode(mazeData.StartCol, mazeData.StartRow);

            InitGlobalExplorationPositions();
            TestGlobalExplorationPositions();
		}

		public void Dispose()
		{
			blockDic.Clear();
			mockNodeSet.Clear();
            globalExplorationNodeList.Clear();

			ToDeleteNodeList.Clear();
			ToCreateNodeList.Clear();
			prevAroundList.Clear();
			mazeTable = null;
			StartNode = null;
			mazeData = null;
		}

		#region Creating and Remove Blocks

        public void Iterate(IterateFunc func)
        {
            if (func == null) { return; }

            Dictionary<int, Block>.Enumerator enumerator = blockDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
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
            AddMockNode(node);

			return block;
		}
		public void RemoveBlock(MazeNode node)
		{
			if (node is MazeRoom)
			{
				MazeRoom room = node as MazeRoom;
				//Prevent deleting same block twice.
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
        public void ClearBlocks()
        {
            Dictionary<int, Block>.Enumerator blockEnum = blockDic.GetEnumerator();
            while(blockEnum.MoveNext())
            {
                Block.Recycle(blockEnum.Current.Value);
            }
            blockDic.Clear();
        }

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

		//Room??
		public Block GetBlockAtPosition(int col, int row)
		{
			if (mazeTable.CheckOccupied(col, row))
			{
                MazeNode node = mazeTable.GetNode(col, row);
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

        public bool CheckInRange(int col, int row)
        {
            return mazeTable.CheckRange(col, row);
        }
        public bool CheckInRange(Vector3 position)
        {
            Vector2 pos = Maze.Instance.GetMazePosition(position);
            int col = (int)pos.x;
            int row = (int)pos.y;
            return mazeTable.CheckRange(col, row);
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

        private HashSet<MazeNode> mockNodeSet = new HashSet<MazeNode>();
        public HashSet<MazeNode> MockNodeSet { get{ return mockNodeSet; } }

        private void AddMockNode(MazeNode node)
		{
            if (!mockNodeSet.Contains(node))
				mockNodeSet.Add(node);
		}

		#endregion

        #region Global Exploration Position Init

        private List<MazeNode> globalExplorationNodeList = new List<MazeNode>();
        public List<MazeNode> GlobalExplorationNodeList { get { return globalExplorationNodeList; } }

        public void InitGlobalExplorationPositions()
        {
            //Calculate how many nodes you need to add global explorations.
            int positionTotalCount = 0;
            foreach(ExplorationType type in mazeData.GlobalExplorationCountDic.Keys)
            {
                int count = mazeData.GlobalExplorationCountDic[type];
                positionTotalCount += count;
            }

            //Get the random indexes in node table.
            List<int> indexList = Utils.GetRandomIndexList(positionTotalCount, mazeTable.NodeCount);

            //Arrange the explorations to the nodes I have created.
            int index = 0;
            foreach (ExplorationType type in mazeData.GlobalExplorationCountDic.Keys)
            {
                int count = mazeData.GlobalExplorationCountDic[type];
                for (int i = 0; i < count; ++i)
                {
                    MazeNode node = mazeTable.GetNode(indexList[index]);
                    node.ExplorationType = type;
                    AddMockNode(node);
                    index++;
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