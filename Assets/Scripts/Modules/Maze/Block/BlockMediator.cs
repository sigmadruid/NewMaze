using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Pathfinding;

namespace GameLogic
{
	public class BlockMediator : Mediator
	{
		private BlockProxy blockProxy;

		private int prevCol;
		private int prevRow;

		public BlockMediator () : base()
		{
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.BLOCK_INIT,
				NotificationEnum.BLOCK_REFRESH,
                NotificationEnum.BLOCK_DISPOSE,
                NotificationEnum.BLOCK_SHOW_ALL,
                NotificationEnum.BLOCK_HIDE_ALL,
			};
		}

		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.BLOCK_INIT:
				{
					HandleBlockInit();
					break;
				}
				case NotificationEnum.BLOCK_DISPOSE:
				{
                    HandleBlockDispose();
					break;
				}
				case NotificationEnum.BLOCK_REFRESH:
				{
					Vector3 position = (Vector3)notification.Body;
					HandleRefreshBlocks(position);
					break;
				}
                case NotificationEnum.BLOCK_SHOW_ALL:
                {
                    HandleShowAllBlocks();
                    break;
                }
                case NotificationEnum.BLOCK_HIDE_ALL:
                {
                    HandleHideAllBlocks();
                    break;
                }
			}
		}

		private void HandleBlockInit()
		{
            prevCol = -1;
            prevRow = -1;

            InitWalkSurface();
		}
		private void HandleBlockDispose()
		{
            blockProxy.Iterate((Block block) =>
                {
                    OnDisposeBlock(block);
                });
            blockProxy.ClearBlocks();
		}

		private void HandleRefreshBlocks(Vector3 position)
		{
            MazePosition pos = Maze.Instance.GetMazePosition(position);
            int col = pos.Col;
            int row = pos.Row;

			if (col == prevCol && row == prevRow)
			{
				return;
			}

			blockProxy.UpdateAroundMazeNode(col, row, GlobalConfig.BlockConfig.RefreshScope);
			List<MazeNode> toDeleteList = blockProxy.ToDeleteNodeList;
			List<MazeNode> toCreateList = blockProxy.ToCreateNodeList;

			for (int i = 0; i < toDeleteList.Count; ++i)
			{
				MazeNode node = toDeleteList[i];
				//Block can be already recycled.
				if (node.AboveBlock != null)
				{
					OnDisposeBlock(node.AboveBlock);
//                    BaseLogger.LogFormat("block: {0},{1}", node.Col, node.Row);
					blockProxy.RemoveBlock(node);
				}
			}
			for (int i = 0; i < toCreateList.Count; ++i)
			{
				MazeNode node = toCreateList[i];
//				Debug.Log(node.Format());
				if (node.AboveBlock == null)
				{
					Block block = blockProxy.AddBlock(node);
					OnInitBlock(block);
				}
			}

//			blockProxy.AddMockNode(col, row);

			prevCol = col;
			prevRow = row;

//			blockProxy.UpdateSearchIndex(col, row);

//            StaticBatchingUtility.Combine(RootTransform.Instance.BlockRoot.gameObject);
		}

        private void HandleShowAllBlocks()
        {
            blockProxy.ShowAllMazeNodes();
            List<MazeNode> toCreateList = blockProxy.ToCreateNodeList;
            for (int i = 0; i < toCreateList.Count; ++i)
            {
                MazeNode node = toCreateList[i];
                if (node.AboveBlock == null)
                {
                    Block block = blockProxy.AddBlock(node);
                    OnInitBlock(block);
                }
            }
        }
        private void HandleHideAllBlocks()
        {
            prevCol = -1;
            prevRow = -1;
            blockProxy.HideAllMazeNodes();
            List<MazeNode> toDeleteList = blockProxy.ToDeleteNodeList;
            for (int i = 0; i < toDeleteList.Count; ++i)
            {
                MazeNode node = toDeleteList[i];
                if (node.AboveBlock != null)
                {
                    Block block = blockProxy.GetBlockAtPosition(node.Col, node.Row);
                    OnDisposeBlock(block);
                    blockProxy.RemoveBlock(node);
                }
            }
        }

		private void OnInitBlock(Block block)
		{
			block.Script.gameObject.isStatic = true;
            DispatchNotification(NotificationEnum.BLOCK_SPAWN, block);
		}
		private void OnDisposeBlock(Block block)
		{
			block.Script.gameObject.isStatic = false;
            DispatchNotification(NotificationEnum.BLOCK_DESPAWN, block);
		}

        private void RefreshRecastGraph()
        {
            MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
            RecastGraph graph = AstarPath.active.graphs[0] as RecastGraph;
            float scope = 2f * GlobalConfig.BlockConfig.RefreshScope + 1f;
            graph.forcedBoundsCenter = new Vector3(Hero.Instance.MazePosition.Col * mazeData.BlockSize, 0, Hero.Instance.MazePosition.Row * mazeData.BlockSize);
            graph.forcedBoundsSize = new Vector3(scope * mazeData.BlockSize, 10, scope * mazeData.BlockSize);
        }
        private void InitWalkSurface()
        {
            blockProxy.ForeachNode((MazeNode node) => 
                {
                    if (node.Data.BlockType == BlockType.Room)
                    {
                        MazeRoom room = node as MazeRoom;
                        if (!room.HasCreated)
                        {
                            GameObject surface = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, node.Data.GetResPath() + "Walk");
                            surface.transform.SetParent(RootTransform.Instance.WalkSurfaceRoot);
                            MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
                            surface.transform.position = MazeUtil.GetWorldPosition(node.Col, node.Row, mazeData.BlockSize);
                            surface.transform.localEulerAngles = Vector3.up * node.Direction * 90f;
                            room.HasCreated = true;
                        }
                    }
                    else if (node.Data.BlockType == BlockType.Passage)
                    {
                        PassageType type = MazeUtil.GetPassageType(node);
                        GameObject surface = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, node.Data.GetResPath() + "Walk");
                        surface.transform.SetParent(RootTransform.Instance.WalkSurfaceRoot);
                        MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
                        surface.transform.position = MazeUtil.GetWorldPosition(node.Col, node.Row, mazeData.BlockSize);
                        surface.transform.localEulerAngles = Vector3.up * node.Direction * 90f;
                    }
                });
            
            RecastGraph graph = AstarPath.active.graphs[0] as RecastGraph;
            graph.SnapForceBoundsToScene();
            AstarPath.active.Scan();

            blockProxy.ForeachNode((MazeNode node) =>
                {
                    if (node.Data.BlockType == BlockType.Room)
                    {
                        MazeRoom room = node as MazeRoom;
                        room.HasCreated = false;
                    }
                });
        }
	}
}

