using System;
using System.Collections.Generic;
namespace StaticData
{
    public class ResourceDataConfig
    {
        public Dictionary<int, List<ResourceData>> DataDic;

        public ResourceDataConfig()
        {
			IDManager idManager = IDManager.Instance;
			BlockConfig blockConfig = GlobalConfig.BlockConfig;

			DataDic = new Dictionary<int, List<ResourceData>>();
			List<ResourceData> dataList;
            ResourceData data;
			int mazeKid;

			dataList = new List<ResourceData>();
			mazeKid = idManager.GetID(IDType.Maze, 0);
			DataDic.Add(mazeKid, dataList);

			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.NPC, 1);
			data.Life = -1;
			data.PreloadCount = 1;
			dataList.Add(data);

			dataList = new List<ResourceData>();
			mazeKid = idManager.GetID(IDType.Maze, 1);
			DataDic.Add(mazeKid, dataList);
			
            data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 1);
			data.Life = -1;
			data.PreloadCount = blockConfig.PassagePreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 1001);
			data.Life = -1;
			data.PreloadCount = blockConfig.PassagePreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 2001);
			data.Life = -1;
			data.PreloadCount = blockConfig.PassagePreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 3001);
			data.Life = -1;
			data.PreloadCount = blockConfig.PassagePreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 4001);
			data.Life = -1;
			data.PreloadCount = blockConfig.PassagePreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 5001);
			data.Life = -1;
			data.PreloadCount = blockConfig.RoomPreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Block, 5002);
			data.Life = -1;
			data.PreloadCount = blockConfig.RoomPreloadCount;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Monster, 1);
			data.Life = -1;
			data.PreloadCount = 10;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Monster, 2);
			data.Life = -1;
			data.PreloadCount = 10;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Monster, 3);
			data.Life = -1;
			data.PreloadCount = 5;
			dataList.Add(data);
			data = new ResourceData();
			data.MazeKid = mazeKid;
			data.EntityKid = idManager.GetID(IDType.Bullet, 1);
			data.Life = -1;
			data.PreloadCount = 10;
			dataList.Add(data);

        }
    }
}

