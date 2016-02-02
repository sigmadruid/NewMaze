using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class StageHomeTown : BaseStage
	{
		public StageHomeTown () : base(StageEnum.HomeTown)
		{
		}

		public override void Start ()
		{
			PreloadAssets();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_INIT);

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_NPC_SPAWN);

			Game.Instance.Looper.SetActive(TaskEnum.InputUpdate, true);
		}
		public override void End ()
		{
			Game.Instance.Looper.SetActive(TaskEnum.InputUpdate, false);

            PopupManager.Instance.Clear();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_DISPOSE);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DISPOSE);

			ResourceManager.Instance.DisposeAssets();
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		private void PreloadAssets()
		{
			int mazeKid = IDManager.Instance.GetID(IDType.Maze, 0);
			List<ResourceData> resourceDataList = ResourceDataManager.Instance.GetResourceDataList(mazeKid);
			
			ResourceManager resManager = ResourceManager.Instance;
			for (int i = 0; i < resourceDataList.Count; ++i)
			{
				ResourceData resourceData = resourceDataList[i];
				IDType idType = IDManager.Instance.GetIDType(resourceData.EntityKid);
				EntityManager manager = GlobalConfig.Instance.GetManager(idType);
				EntityData data = manager.GetData(resourceData.EntityKid);
				resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.Life, resourceData.PreloadCount);
				
			}

			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/HPBar", -1, 1);
			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/NPCIcon", -1, 1);
		}
	}
}

