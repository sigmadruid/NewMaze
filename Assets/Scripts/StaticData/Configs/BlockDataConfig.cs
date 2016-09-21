using UnityEngine;
using System;
using System.Collections.Generic;


namespace StaticData
{
	public class BlockDataConfig
	{
		public Dictionary<PassageType, List<BlockData>> PassageDataDic;
		public List<BlockData> RoomDataList;
		public Dictionary<int, BlockData> BlockDataDic;

		public BlockDataConfig()
		{
			IDManager idManager = IDManager.Instance;
			PassageDataDic = new Dictionary<PassageType, List<BlockData>>();
			RoomDataList = new List<BlockData>();
			BlockDataDic = new Dictionary<int, BlockData>();
			List<BlockData> list;
			BlockData data;

			//----One----
			list = new List<BlockData>();
			PassageDataDic.Add(PassageType.One, list);

			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 1);
			data.BlockType = BlockType.Passage;
			data.PassageType = PassageType.One;
			data.Res3D = "PassageOne";
			list.Add(data);

			//----TwoLine----
			list = new List<BlockData>();
			PassageDataDic.Add(PassageType.TwoLine, list);
			
			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 1201);
			data.BlockType = BlockType.Passage;
			data.PassageType = PassageType.TwoLine;
			data.Res3D = "PassageTwoLine";
			list.Add(data);

			//----TwoTurn----
			list = new List<BlockData>();
			PassageDataDic.Add(PassageType.TwoTurn, list);
			
			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 2401);
			data.BlockType = BlockType.Passage;
			data.PassageType = PassageType.TwoTurn;
			data.Res3D = "PassageTwoTurn";
			list.Add(data);

			//----Three----
			list = new List<BlockData>();
			PassageDataDic.Add(PassageType.Three, list);
			
			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 3601);
			data.BlockType = BlockType.Passage;
			data.PassageType = PassageType.Three;
			data.Res3D = "PassageThree";
			list.Add(data);

			//----Four----
			list = new List<BlockData>();
			PassageDataDic.Add(PassageType.Four, list);
			
			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 4801);
			data.BlockType = BlockType.Passage;
			data.PassageType = PassageType.Four;
			data.Res3D = "PassageFour";
			list.Add(data);

			//----Room----
			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 6001);
			data.BlockType = BlockType.Room;
			data.PassageType = PassageType.One;
			data.Res3D = "Room_1";
			data.Cols = 3;
			data.Rows = 3;
			data.LeftOffset = 2;
			RoomDataList.Add(data);

			data = new BlockData();
			data.Kid = idManager.GetID(IDType.Block, 6002);
			data.BlockType = BlockType.Room;
			data.PassageType = PassageType.One;
			data.Res3D = "Room_2";
			data.Cols = 5;
			data.Rows = 5;
			data.LeftOffset = 3;
			RoomDataList.Add(data);

			foreach(List<BlockData> dataList in PassageDataDic.Values)
			{
				for (int i = 0; i < dataList.Count; ++i)
				{
					data = dataList[i];
					BlockDataDic.Add(data.Kid, data);
				}
			}
			for (int i = 0; i < RoomDataList.Count; ++i)
			{
				data = RoomDataList[i];
				BlockDataDic.Add(data.Kid, data);
			}
			
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Start;
//			data.Length = 10;
//			data.Width = 10;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Level_Enter";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Final;
//			data.Length = 15;
//			data.Width = 15;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Level_Exit";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Dungeons_Module1A";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Dungeons_Module1B";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Dungeons_Module1C";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Dungeons_Module1D";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 30;
//			data.Width = 30;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Courner1A";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 30;
//			data.Width = 30;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Courner1B";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Passage;
//			data.Length = 30;
//			data.Width = 30;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Courner1C";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room1";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room2";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 10;
//			data.Width = 10;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room3A";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 10;
//			data.Width = 10;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room3B";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 10;
//			data.Width = 10;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room3C";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 15;
//			data.Width = 15;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room4";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 20;
//			data.Width = 20;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room_down1";
//			BlockDataDic.Add(data.Kid, data);
//
//			data = new BlockData();
//			data.Kid = idManager.GetID(IDType.Block, index++);
//			data.Type = BlockType.Room;
//			data.Length = 25;
//			data.Width = 25;
//			data.Height = 3;
//			data.Res3D = "MazeBlocks/Room_down2";
//			BlockDataDic.Add(data.Kid, data);
		}
	}
}

