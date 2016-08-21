using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class MazeMain : MonoBehaviour
{
    #region Public Proterties

    public int Seed;

    public MazeData mazeData;

    #endregion

    private BlockProxy blockProxy;

	void Awake()
	{
		ConfigManager.Instance.InitAllData();
        Maze.Instance.Init();

        blockProxy = new BlockProxy();
        ApplicationFacade.Instance.RegisterProxy(blockProxy);


        ApplicationFacade.Instance.RegisterMediator(new BlockMediator());

        mazeData = MazeDataManager.Instance.CurrentMazeData;
        PreloadAssets(mazeData.Kid);
    }

    public void GenerateMaze(int seed)
    {
        ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_DISPOSE);

        Seed = seed;
        RandomUtils.Seed = seed;
        Vector3 startPosition = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);

        blockProxy.Init();
        ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_INIT);
        ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_SHOW_ALL);
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
                resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.Life, resourceData.PreloadCount);
            }
            else
            {
                resManager.PreloadAsset(ObjectType.GameObject, resourceData.Path, resourceData.Life, resourceData.PreloadCount);
            }
        }
    }
}

