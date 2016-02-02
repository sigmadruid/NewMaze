using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

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
				
			}
		}

		private void HandleBlockInit()
		{
			MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
			Vector3 position = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);
            prevCol = 0;
            prevRow = 0;
			HandleRefreshBlocks(position);
		}
		private void HandleBlockDispose()
		{

		}

		private void HandleRefreshBlocks(Vector3 position)
		{
			int col, row;
			float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
			MazeUtil.GetMazePosition(position, blockSize, out col, out row);

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
//					Logger.Log("block: ", block.Col, block.Row);
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

			blockProxy.AddMockNode(col, row);

			prevCol = col;
			prevRow = row;

			blockProxy.UpdateSearchIndex(col, row);

//			Block block = blockProxy.GetBlockAtPosition(position);
//			if (block != currentBlock)
//			{
//				if (currentBlock != null && currentBlock.Script != null)
//				{
//					ConjunctionScript[] currentConjunctionList = currentBlock.Script.ConjunctionList;
//					for (int i = 0; i < currentConjunctionList.Length; ++i)
//					{
//						ConjunctionScript currentConjunction = currentConjunctionList[i];
//						if (currentConjunction.NeighborBlock != block)
//						{
//							OnDisposeBlock(currentConjunction.NeighborBlock);
//
//							Block.RecycleBlock(currentConjunction.NeighborBlock);
//							blockProxy.RemoveBlock(currentConjunction.NeighborBlock.Uid);
//
//							currentConjunction.NeighborBlock = null;
//						}
//					}
//				}
//				//TODO: Check this. Why?
//				if (block != null)
//				{
//					ConjunctionScript[] conjunctionList = block.Script.ConjunctionList;
//					for (int i = 0; i < conjunctionList.Length; ++i)
//					{
//						ConjunctionScript conjunction = conjunctionList[i];
//						if (conjunction.NeighborBlock == null)
//						{
//							Block newBlock = CreateBlock(block.Data.Type);
//							ConnectBlocks(newBlock, block, conjunction);
//							blockProxy.AddBlock(newBlock);
//
//							OnInitBlock(newBlock);
//						}
//					}
//				}
//
//				mazeProxy.CurrentMaze.CurrentBlockCount++;
//				currentBlock = block;
//			}

		}

//		private void ConnectBlocks(Block newBlock, Block block, ConjunctionScript conjunction)
//		{
//			ConjunctionScript newConjunction = newBlock.Script.GetAvailableConjunction();
//
//			newBlock.SetPosition(conjunction.Position - newConjunction.LocalPosition);
//
//			float newAngle = Quaternion.LookRotation(newConjunction.Direction).eulerAngles.y;
//			float angle = Quaternion.LookRotation(conjunction.Direction).eulerAngles.y;
//			newBlock.SetRotation(180 - (newAngle - angle), conjunction.Position);
//
//			newConjunction.NeighborBlock = block;
//			conjunction.NeighborBlock = newBlock;
//		}

		private void OnInitBlock(Block block)
		{
			block.Script.gameObject.isStatic = true;
			DispatchNotification(NotificationEnum.MONSTER_SPAWN, block);
			DispatchNotification(NotificationEnum.NPC_SPAWN, block);
			DispatchNotification(NotificationEnum.EXPLORATION_SPAWN, block);
			DispatchNotification(NotificationEnum.DROP_SPAWN, block);
		}
		private void OnDisposeBlock(Block block)
		{
			block.Script.gameObject.isStatic = false;
			DispatchNotification(NotificationEnum.MONSTER_DESPAWN, block);
			DispatchNotification(NotificationEnum.NPC_DESPAWN, block);
			DispatchNotification(NotificationEnum.EXPLORATION_DESPAWN, block);
			DispatchNotification(NotificationEnum.DROP_DESPAWN, block);
		}
	}
}

