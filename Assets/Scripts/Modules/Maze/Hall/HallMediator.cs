using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Pathfinding;

namespace GameLogic
{
    public class HallMediator : Mediator
    {
		private HallProxy hallProxy;
        private GameObject walkSurface;

		public override void OnRegister ()
		{
			hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
		}
		
		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.HALL_INIT,
				NotificationEnum.HALL_DISPOSE,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
			switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.HALL_INIT:
				{
                    HandleHallInit(notification.Body);
					break;
				}
				case NotificationEnum.HALL_DISPOSE:
				{
					HandleHallDispose();
					break;
				}
			}
		}

        private void HandleHallInit(object param)
		{
            if(param is HallRecord)
            {
                HallRecord record = param as HallRecord;
                InitWalkSurface(record.Kid);
                Hall.Create(record);
                Hall.Instance.LeavePosition = record.LeavePosition.ToVector3();
            }
            else
            {
                int hallKid = (int)param;
                InitWalkSurface(hallKid);
                Hall.Create(hallKid);
            }
            DispatchNotification(NotificationEnum.HALL_SPAWN, Hall.Instance);
		}
		
		private void HandleHallDispose()
		{
            DispatchNotification(NotificationEnum.HALL_DESPAWN, Hall.Instance);
            Hall.Recycle(Hall.Instance);
            DisposeWalkSurface();
		}

        private void InitWalkSurface(int kid)
        {
            HallData data = HallDataManager.Instance.GetData(kid) as HallData;
            TextAsset ta = Resources.Load<TextAsset>(data.GetResPath() + GlobalConfig.PathfindingConfig.WalkDataSuffix);
            AstarPath.active.astarData.DeserializeGraphs(ta.bytes);
        }
        private void DisposeWalkSurface()
        {
        }
    }
}

