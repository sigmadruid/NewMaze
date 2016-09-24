using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;
using Base;


namespace GameLogic
{
    public class Hall : Entity
    {
        public Vector3 LeavePosition;

        public new HallData Data
        {
            get { return data as HallData; }
            protected set { data = value; }
        }

        public new HallScript Script
        {
            get { return script as HallScript; }
            protected set { script = value; }
        }

		private static Hall instance;
		public static Hall Instance { get { return instance; } }

        public new HallRecord ToRecord()
        {
            HallRecord record = new HallRecord();
            record.Kid = Data.Kid;
            record.LeavePosition = new Vector3Record(LeavePosition);
            return record;
        }

        public static Hall Create(HallRecord record)
        {
            return Create(record.Kid);
        }
		public static Hall Create(int kid)
		{
			ResourceManager resManager = ResourceManager.Instance;

			Hall hall = new Hall();
			hall.Uid = Guid.NewGuid().ToString();
			hall.Data = HallDataManager.Instance.GetData(kid) as HallData;
			hall.Script = resManager.LoadAsset<HallScript>(ObjectType.GameObject, hall.Data.GetResPath());
            hall.Script.Uid = hall.Uid;
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

