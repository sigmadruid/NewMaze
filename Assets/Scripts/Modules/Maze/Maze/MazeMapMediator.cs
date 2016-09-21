using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Effects;

namespace GameLogic
{
    public class MazeMapMediator : Mediator
    {
		private const string PANEL_PATH = "UI/MazeMapPanel";

		private bool show;

		private MazeMapPanel panel;

		private BlockProxy blockProxy;

        private HashSet<MazeNode> mockBlockSet = new HashSet<MazeNode>();

        public MazeMapMediator() : base()
        {
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
            Game.Instance.SetPause(show);

			if (show)
			{
				panel = PopupManager.Instance.CreateAndAddPopup<MazeMapPanel>();

				BuildMockBlock();

                Vector3 position = GetHeroMazeMapPosition(Hero.Instance.WorldPosition);
                panel.Show(true, position, Hero.Instance.WorldAngle);		
			}
			else
			{
				PopupManager.Instance.RemovePopup(panel);
				panel.Show(false, Vector3.zero, 0f);
			}
            DispatchNotification(NotificationEnum.ENVIRONMENT_SHOW_MAZE_MAP, show);
		}
		private void HandleMazeMapReset()
		{
            mockBlockSet.Clear();
		}

        private Vector3 GetHeroMazeMapPosition(Vector3 heroPosition)
        {
            Vector2 heroPos = Maze.Instance.GetMazePosition(heroPosition);
            float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
            float posY = GlobalConfig.BlockConfig.MockBlockPosY;
            Vector3 position = new Vector3(heroPos.x * cubeSize, posY, heroPos.y * cubeSize);
            return position;
        }

		private void BuildMockBlock()
		{
            HashSet<MazeNode> nodeSet = blockProxy.MockNodeSet;
            foreach(MazeNode node in nodeSet)
			{
                if(mockBlockSet.Contains(node)) continue;

				if (node is MazeRoom)
				{
					CreateMockRoom(node as MazeRoom);
				}
				else
				{
					CreateMockPassage(node);
				}

                if(node.ExplorationType != ExplorationType.Common)
                {
                    CreateIndicator(node);
                }
                mockBlockSet.Add(node);
			}
		}

		private void CreateMockRoom(MazeRoom room)
		{
			float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;

			GameObject cube = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockRoomPath);
            cube.transform.localScale = new Vector3(room.Data.Cols * cubeSize, 0.1f, room.Data.Rows * cubeSize);
			cube.transform.localEulerAngles = Vector3.up * 90f * room.Direction;
			cube.transform.position = new Vector3(room.Col * cubeSize , GlobalConfig.BlockConfig.MockBlockPosY, room.Row * cubeSize);
			cube.transform.parent = RootTransform.Instance.MockBlockRoot;
            cube.transform.position = cube.transform.position - cube.transform.forward * GlobalConfig.BlockConfig.MockLinkSize * 0.5f;
            cube.GetComponentInChildren<MeshRenderer>().material.mainTextureScale = new Vector2(room.Data.Cols, room.Data.Rows);
            cube.GetComponent<Sparking>().IsEnabled = room.ExplorationType != ExplorationType.Common;
		}
		private void CreateMockPassage(MazeNode node)
		{
			float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
			float posY = GlobalConfig.BlockConfig.MockBlockPosY;

			GameObject cube = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockPassagePath);
			cube.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize);
			cube.transform.parent = RootTransform.Instance.MockBlockRoot;
            cube.GetComponent<Sparking>().IsEnabled = node.ExplorationType != ExplorationType.Common;

			GameObject link = null;
            float offsetSize = cubeSize * 0.5f;
			if (node.LinkList[0] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
                link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize + offsetSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[1] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
                link.transform.position = new Vector3(node.Col * cubeSize + offsetSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[2] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
                link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize - offsetSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
			if (node.LinkList[3] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
                link.transform.position = new Vector3(node.Col * cubeSize - offsetSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
			}
		}

        private void CreateIndicator(MazeNode node)
        {
            float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
            float posY = GlobalConfig.BlockConfig.MockBlockPosY;

            GameObject indicator = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.IndicatorPath);
            indicator.transform.position = new Vector3(node.Col * cubeSize, posY + 2f, node.Row * cubeSize);
            indicator.transform.parent = RootTransform.Instance.MockBlockRoot;
        }

    }
}

