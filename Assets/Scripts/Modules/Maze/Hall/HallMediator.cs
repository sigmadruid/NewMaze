using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class HallMediator : Mediator
    {
		private HallProxy hallProxy;

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
                    int hallKid = (int)notification.Body;
                    HandleHallInit(hallKid);
					break;
				}
				case NotificationEnum.HALL_DISPOSE:
				{
					HandleHallDispose();
					break;
				}
			}
		}

        private void HandleHallInit(int hallKid)
		{
            if(hallProxy.CurrentHall == null)
            {
                hallProxy.CurrentHall = Hall.Create(hallKid);
            }
            DispatchNotification(NotificationEnum.HALL_SPAWN, hallProxy.CurrentHall);
		}
		
		private void HandleHallDispose()
		{
            DispatchNotification(NotificationEnum.HALL_DESPAWN, hallProxy.CurrentHall);
            Hall.Recycle(hallProxy.CurrentHall);
            hallProxy.CurrentHall = null;
		}
    }
}

