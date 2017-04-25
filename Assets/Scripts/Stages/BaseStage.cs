using System;
using System.Collections;
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

        public abstract IEnumerator Start();

        public abstract IEnumerator End();

		public static BaseStage CreateStage(StageEnum stageEnum)
		{
			switch(stageEnum)
			{
				case StageEnum.HomeTown: return new StageHomeTown();
				case StageEnum.Maze: return new StageMaze();
				default: return null;
			}
		}

        protected IEnumerator PreloadAssets(int mazeKid)
        {
            List<ResourceData> resourceDataList = ResourceDataManager.Instance.GetResourceDataList(mazeKid);

            ResourceManager resManager = ResourceManager.Instance;
            resManager.InitAssets();
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
                    resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.PreloadCount);
                    yield return Loading.Instance.SetProgress(LoadingState.StartStage, 1);
                }
                else
                {
                    resManager.PreloadAsset(ObjectType.GameObject, resourceData.Path, resourceData.PreloadCount);
                    yield return Loading.Instance.SetProgress(LoadingState.StartStage, 1);
                }
            }
            yield break;
        }
	}
}

