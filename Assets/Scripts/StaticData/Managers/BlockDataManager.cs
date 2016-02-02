using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
	public class BlockDataManager : EntityManager
	{
		private Dictionary<PassageType, List<BlockData>> passageDic;
		private List<BlockData> roomList;
		private Dictionary<int, BlockData> kvDic;

		private static BlockDataManager instance;
		public static BlockDataManager Instance
		{
			get
			{
				if (instance == null) instance = new BlockDataManager();
				return instance;
			}
		}

		public void Init()
		{
			BlockDataParser parser = new BlockDataParser();
			parser.Parse("BlockDataConfig.csv", out passageDic, out roomList, out kvDic);
		}

		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such kid in BlockDataDic: {0}", kid);
			}
			return kvDic[kid];
		}

		public List<BlockData> GetPassageDataList(PassageType type)
		{
			return passageDic[type];
		}
		public List<BlockData> GetRoomDataList()
		{
			return roomList;
		}

		public BlockData GetRandomPassageData(PassageType type)
		{
			List<BlockData> list = passageDic[type];
			int index = RandomUtils.Range(0, list.Count);
			return list[index];
		}

		public BlockData GetRandomRoomData()
		{
			int index = RandomUtils.Range(0, roomList.Count);
			return roomList[index];
		}


	}
}

