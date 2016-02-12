using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;
using Base;


namespace GameLogic
{
    public class Hall
    {
		public string Uid;

		public HallScript Script;
		public HallData Data;

		public Vector3 WorldPosition;

		private static Hall instance;
		public static Hall Instance { get { return instance; } }

		public void SetPosition(Vector3 position)
		{
			WorldPosition = position;
			Script.transform.position = position;
		}

		public static Hall Create(int kid)
		{
			ResourceManager resManager = ResourceManager.Instance;

			Hall hall = new Hall();
			hall.Uid = Guid.NewGuid().ToString();
			hall.Data = HallDataManager.Instance.GetData(kid) as HallData;
			hall.Script = resManager.LoadAsset<HallScript>(ObjectType.GameObject, hall.Data.GetResPath());
			hall.SetPosition(GlobalConfig.HallConfig.HallPosition);
			instance = hall;

			return hall;
		}

		public static void Recycle(Hall hall)
		{
			if (hall != null)
			{
                instance = null;
				hall.Data = null;
				ResourceManager.Instance.RecycleAsset(hall.Script.gameObject);
				hall.Script = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null hall!");
			}
		}
    }
}

