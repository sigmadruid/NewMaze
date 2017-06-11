using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Effects;
using DG.Tweening;

namespace GameLogic
{
    public class MazeMapMediator : Mediator
    {
		private MazeMapPanel panel;

		private BlockProxy blockProxy;

        private HashSet<MazeNode> mockBlockSet = new HashSet<MazeNode>();

        private bool show;
        private const float SUNK_HEIGHT = 5f;
        private const float TOTAL_DELAY = 0.2f;
        private const float ANIM_DURATION = 0.2f;
        private float delay;

        public MazeMapMediator() : base()
        {
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
        }

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
                NotificationEnum.KEY_DOWN,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
                case NotificationEnum.KEY_DOWN:
				{
                    KeyboardActionType actionType = (KeyboardActionType)notification.Body;
                    HandleMockBlockShow(actionType);
					break;
				}
			}
		}

        private void HandleMockBlockShow(KeyboardActionType actionType)
		{
            if(actionType != KeyboardActionType.MazeMap) return;

			show = !show;

			if (show)
			{
                panel = PopupManager.Instance.CreateAndAddPopup<MazeMapPanel>(PopupMode.SHOW | PopupMode.ADD_MASK);
                Vector3 position = GetHeroMazeMapPosition(Adam.Instance.WorldPosition);
                panel.Show(true, position, Adam.Instance.WorldAngle);	
                BuildMockBlock();
			}
			else
			{
                panel.Show(false, Vector3.zero, 0);  
				PopupManager.Instance.RemovePopup(panel);
			}
            DispatchNotification(NotificationEnum.ENVIRONMENT_SHOW_MAZE_MAP, show);
		}
		private void HandleMazeMapReset()
		{
            mockBlockSet.Clear();
		}

        private Vector3 GetHeroMazeMapPosition(Vector3 heroPosition)
        {
            MazePosition heroPos = Maze.Instance.GetMazePosition(heroPosition);
            float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
            float posY = GlobalConfig.BlockConfig.MockBlockPosY;
            Vector3 position = new Vector3(heroPos.Col * cubeSize, posY, heroPos.Row * cubeSize);
            return position;
        }

		private void BuildMockBlock()
		{
            HashSet<MazeNode> nodeSet = blockProxy.MockNodeSet;
            delay = 0;
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

                if(node.ExplorationKid != 0)
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
            cube.transform.position = cube.transform.position - cube.transform.forward * GlobalConfig.BlockConfig.MockLinkSize * 0.5f - Vector3.up * SUNK_HEIGHT;
            cube.GetComponentInChildren<MeshRenderer>().material.mainTextureScale = new Vector2(room.Data.Cols, room.Data.Rows);
            cube.GetComponent<Sparking>().IsEnabled = room.ExplorationKid != 0;
            CreateAnim(cube, true);
		}
		private void CreateMockPassage(MazeNode node)
		{
			float cubeSize = GlobalConfig.BlockConfig.MockCubeSize;
            float posY = GlobalConfig.BlockConfig.MockBlockPosY - SUNK_HEIGHT;

			GameObject cube = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockPassagePath);
			cube.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize);
			cube.transform.parent = RootTransform.Instance.MockBlockRoot;
            cube.GetComponent<Sparking>().IsEnabled = node.ExplorationKid != 0;
            CreateAnim(cube, true);

			GameObject link = null;
            float offsetSize = cubeSize * 0.5f;
			if (node.LinkList[0] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
                link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize + offsetSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
                CreateAnim(link, false);
                node.LinkList[0].LinkList[MazeUtil.GetOppositeDirection(0)] = null;
			}
			if (node.LinkList[1] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
                link.transform.position = new Vector3(node.Col * cubeSize + offsetSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
                CreateAnim(link, false);
                node.LinkList[1].LinkList[MazeUtil.GetOppositeDirection(1)] = null;
			}
			if (node.LinkList[2] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
				link.transform.localEulerAngles = Vector3.up * 90;
                link.transform.position = new Vector3(node.Col * cubeSize, posY, node.Row * cubeSize - offsetSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
                CreateAnim(link, false);
                node.LinkList[2].LinkList[MazeUtil.GetOppositeDirection(2)] = null;
			}
			if (node.LinkList[3] != null)
			{
				link = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath);
                link.transform.position = new Vector3(node.Col * cubeSize - offsetSize, posY, node.Row * cubeSize);
				link.transform.parent = RootTransform.Instance.MockBlockRoot;
                CreateAnim(link, false);
                node.LinkList[3].LinkList[MazeUtil.GetOppositeDirection(3)] = null;
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

        private void CreateAnim(GameObject go, bool addDelay)
        {
            if (addDelay)
                delay += TOTAL_DELAY / blockProxy.MockNodeSet.Count;
            Vector3 currentPosition = go.transform.localPosition;
            go.transform.DOLocalMove(currentPosition + Vector3.up * 5, 1).SetDelay(delay);
            go.GetComponent<Fade>().StartEffect();
        }

    }
}

