using System;
using System.Collections.Generic;

namespace StaticData
{
    public class ExplorationDataConfig
    {
        public Dictionary<int, ExplorationData> DataDic;

        public ExplorationDataConfig()
        {
			IDManager idManager = IDManager.Instance;
			DataDic = new Dictionary<int, ExplorationData>();
			ExplorationData data;

			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 1);
			data.Type = ExplorationType.Chest;
			data.Res3D = "Chest_1";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 2);
			data.Type = ExplorationType.Chest;
			data.Res3D = "Chest_2";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 3);
			data.Type = ExplorationType.Chest;
			data.Res3D = "Chest_3";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 4);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_1";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 5);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_2";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 6);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_3";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 7);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_4";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 8);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_5";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 9);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_6";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 10);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_7";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 11);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_8";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
			data = new ExplorationData();
			data.Kid = idManager.GetKid(IDType.Exploration, 12);
			data.Type = ExplorationType.Corpse;
			data.Res3D = "Corpse_9";
			data.MazeKid = idManager.GetKid(IDType.Maze, 1);
			DataDic.Add(data.Kid, data);
			
        }
    }
}

