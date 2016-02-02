using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class MazeMapMediator : Mediator
    {
		private const string PANEL_PATH = "UI/MazeMapPanel";

		private bool show;

		private MazeMapPanel panel;

		private BlockProxy blockProxy;

		private HashSet<int> posHashSet;

        public MazeMapMediator() : base()
        {
			posHashSet = new HashSet<int>();
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
        }

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.MAZE_MAP_SHOW,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.MAZE_MAP_SHOW:
				{
					HandleMockBlockShow();
					break;
				}
				case NotificationEnum.MAZE_MAP_RESET:
				{
					HandleMazeMapReset();
					break;
				}
			}
		}

		private void HandleMockBlockShow()
		{
			show = !show;

			if (show)
			{
				panel = PopupManager.Instance.CreateAndAddPopup<MazeMapPanel>();

				BuildMockBlock();
				Vector2 heroPos = Maze.Instance.GetMazePosition(Hero.Instance.WorldPosition);
				float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
				float posY = GlobalConfig.BlockConfig.MockBlockPosY;
				Vector3 position = new Vector3(heroPos.x * cubeSize, posY, heroPos.y * cubeSize);
				panel.Show(true, position);		
			}
			else
			{
				PopupManager.Instance.RemovePopup(panel);
				panel.Show(false, Vector3.zero);
			}
		}
		private void HandleMazeMapReset()
		{
			posHashSet.Clear();
		}

		private void BuildMockBlock()
		{
			List<MazeNode> nodeList = blockProxy.MockNodeList;
			int count = nodeList.Count;
			for (int i = 0; i < count; ++i)
			{
				MazeNode node = nodeList[i];
				if (node is MazeRoom)
				{
					MazeRoom room = node as MazeRoom;
					if (!room.HasCreated)
					{
						BuildRoomCube(node as MazeRoom);
						room.HasCreated = true;
					}
				}
				else
				{
					BuildPassageCube(node);
				}
			}
			nodeList.Clear();
		}

		private void BuildRoomCube(MazeRoom room)
		{
			int key = Block.GetBlockKey(room.Col, room.Row);
			if (posHashSet.Contains(key))
		    {
				return;
			}
			float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;

			GameObject cube = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockRoomPath);
			BlockData data = BlockDataManager.Instance.GetRandomRoomData();
			cube.transform.localScale = new Vector3(data.Cols, 1, data.Rows);
			cube.transform.localEulerAngles = Vector3.up * 90f * room.Direction;
			cube.transform.position = new Vector3(room.Col * cubeSize , GlobalConfig.BlockConfig.MockBlockPosY, room.Row * cubeSize);
			cube.transform.parent = RootTransform.Instance.MockBlockRoot;

			posHashSet.Add(key);
		}
		private void BuildPassageCube(MazeNode node)
		{
			int key = Block.GetBlockKey(node.Col, node.Row);
			if (posHashSet.Contains(key))
			{
				return;
			}

			float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
			float linkSize = GlobalConfig.BlockConfig.MockLinkSize;
			float posY = GlobalConfig.BlockConfig.MockBlockPosY;

			GameObject cube = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockPassagePath);
			cube.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize);
			cube.transform.parent = RootTransform.Instance.MockBlockRoot;
			
			GameObject link = null;
			if (node.LinkList[0] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
				link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize + linkSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[1] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.position = new Vector3(node.Col * cubeSize + linkSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[2] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
				link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize - linkSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[3] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.position = new Vector3(node.Col * cubeSize - linkSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}

			posHashSet.Add(key);
		}

    }
}

