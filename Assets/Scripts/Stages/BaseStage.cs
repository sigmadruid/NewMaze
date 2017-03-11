using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public enum StageEnum
	{
		HomeTown,
		Maze,
	}

	public abstract class BaseStage
	{
		public BaseStage (StageEnum stageEnum) { Type = stageEnum; }

		public StageEnum Type { get; protected set; }

		public abstract void Start();

		public abstract void End();

		public static BaseStage CreateStage(StageEnum stageEnum)
		{
			switch(stageEnum)
			{
				case StageEnum.HomeTown: return new StageHomeTown();
				case StageEnum.Maze: return new StageMaze();
				default: return null;
			}
		}

        protected void PreloadAssets(int mazeKid)
        {
            List<ResourceData> resourceDataList = ResourceDataManager.Instance.GetResourceDataList(mazeKid);

            ResourceManager resManager = ResourceManager.Instance;
            for(int i = 0; i < resourceDataList.Count; ++i)
            {
                ResourceData resourceData = resourceDataList[i];
                if(resourceData.EntityKid != 0)
                {
                    IDType idType = IDManager.Instance.GetIDType(resourceData.EntityKid);
                    EntityManager manager = GlobalConfig.Instance.GetManager(idType);
                    EntityData data = manager.GetData(resourceData.EntityKid);
                    if(data == null)
                    {
                        BaseLogger.LogFormat("Can't find data {0}", resourceData.EntityKid);
                    }
                    resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.Life, resourceData.PreloadCount);
                }
                else
                {
                    resManager.PreloadAsset(ObjectType.GameObject, resourceData.Path, resourceData.Life, resourceData.PreloadCount);
                }
            }
        }
	}
}

