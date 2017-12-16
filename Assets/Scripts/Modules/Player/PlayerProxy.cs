using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    //Singlton??
    public class PlayerProxy : Proxy
    {
        public PlayerInfo CurrentInfo;

        public void Init(PlayerRecord record)
		{
            CurrentInfo = null;
            if(record != null)
            {
                CurrentInfo = new PlayerInfo();
                CurrentInfo.Init(record);
            }
            else
            {
                CurrentInfo = new PlayerInfo();
                CurrentInfo.Uid = Guid.NewGuid().ToString();
                CurrentInfo.Name = "Demo Adam";
                CurrentInfo.HeroKid = 30001;
                MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
                Vector3 startPosition = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);
                CurrentInfo.StartPosition = startPosition;
                CurrentInfo.StartAngle = 0f;

                CurrentInfo.IsConverting = false;
                CurrentInfo.IsInHall = false;
                CurrentInfo.IsVisible = true;

                CurrentInfo.HallKid = 0;
                CurrentInfo.LeavePosition = Vector3.zero;
            }
		}

        public PlayerRecord Save()
        {
            PlayerRecord record = CurrentInfo.ToRecord();
            return record;
        }

    }
}

