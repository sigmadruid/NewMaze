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
                Hall.Create(record);
                Hall.Instance.LeavePosition = record.LeavePosition.ToVector3();
            }
            else
            {
                int hallKid = (int)param;
                Hall.Create(hallKid);
            }
            DispatchNotification(NotificationEnum.PATHFINDING_INIT, PathfindingType.Hall);
            DispatchNotification(NotificationEnum.HALL_SPAWN, Hall.Instance);
		}
		
		private void HandleHallDispose()
		{
            DispatchNotification(NotificationEnum.HALL_DESPAWN, Hall.Instance);
            DispatchNotification(NotificationEnum.PATHFINDING_DISPOSE, true);
            Hall.Recycle(Hall.Instance);
		}

    }
}

