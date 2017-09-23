using UnityEngine;

using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class PlayerInfo
    {
		public string Uid;

		public string Name;
        public int HeroKid;
        public Vector3 StartPosition;
        public float StartAngle;

        public bool IsConverting;
        public bool IsInHall;
        public bool IsVisible;

        public int HallKid;
        public Vector3 LeavePosition;

        public void Init()
        {
        }
        public void Init(PlayerRecord record)
		{
			Uid = record.Uid;
			Name = record.Name;

            HeroKid = record.HeroKid;
            StartPosition = record.StartPosition.ToVector3();
            StartAngle = record.StartAngle;

            IsConverting = record.IsConverting;
            IsInHall = record.IsInHall;
            IsVisible = record.IsVisible;

            HallKid = record.HallKid;
            LeavePosition = record.LeavePosition.ToVector3();
		}

        public PlayerRecord ToRecord()
        {
            PlayerRecord record = new PlayerRecord();
            record.Uid = Uid;
            record.Name = Name;
            record.HeroKid = HeroKid;
            record.StartPosition = new Vector3Record(Adam.Instance.WorldPosition);
            record.StartAngle = Adam.Instance.WorldAngle;

            record.IsConverting = IsConverting;
            record.IsInHall = IsInHall;
            record.IsVisible = IsVisible;

            record.HallKid = HallKid;
            record.LeavePosition = new Vector3Record(LeavePosition);
            return record;
        }
    }
}

