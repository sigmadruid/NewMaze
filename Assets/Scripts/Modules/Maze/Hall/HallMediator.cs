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
		private Hall currentHall;

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
					HandleHallInit();
					break;
				}
				case NotificationEnum.HALL_DISPOSE:
				{
					HandleHallDispose();
					break;
				}
			}
		}

		private void HandleHallInit()
		{
			if (currentHall == null)
				currentHall = Hall.Create(110001);
            DispatchNotification(NotificationEnum.HALL_SPAWN, currentHall);
		}
		
		private void HandleHallDispose()
		{
            DispatchNotification(NotificationEnum.HALL_DESPAWN, currentHall);
			Hall.Recycle(currentHall);
		}
    }
}

