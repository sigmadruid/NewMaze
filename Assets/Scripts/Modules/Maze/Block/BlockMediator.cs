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
        private bool walkSurfaceLoaded;

		public BlockMediator () : base()
		{
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
                NotificationEnum.BLOCK_SPAWN,
                NotificationEnum.BLOCK_DESPAWN,
				NotificationEnum.BLOCK_INIT,
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
                case NotificationEnum.BLOCK_SPAWN:
                    {
                        HandleBlockSpawn();
                        break;
                    }
                case NotificationEnum.BLOCK_DESPAWN:
                    {
                        HandleBlockDespawn();
                        break;
                    }
			}
		}

		private void HandleBlockInit()
		{
            blockProxy.ForeachBlockNodes((MazeNode node) => 
                {
                    blockProxy.AddBlock(node);
                });
            DispatchNotification(NotificationEnum.PATHFINDING_INIT, PathfindingType.Maze);
		}
		private void HandleBlockDispose()
		{
            DispatchNotification(NotificationEnum.PATHFINDING_DISPOSE, false);
            blockProxy.ClearBlocks();
		}
        private void HandleBlockSpawn()
        {
            DispatchNotification(NotificationEnum.PATHFINDING_INIT, PathfindingType.Maze);
        }
        private void HandleBlockDespawn()
        {
            DispatchNotification(NotificationEnum.PATHFINDING_DISPOSE, false);
        }

	}
}

