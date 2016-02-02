using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class BulletMediator : Mediator
    {
		private BulletProxy bulletProxy;

		public BulletMediator () : base()
		{
			bulletProxy = ApplicationFacade.Instance.RetrieveProxy<BulletProxy>();
		}
		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.BULLET_SPAWN,
				NotificationEnum.BULLET_DESPAWN,
				NotificationEnum.BATTLE_PAUSE,
			};
		}
		public override void HandleNotification (INotification notification)
		{
			switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.BULLET_SPAWN:
				{
					Bullet bullet = notification.Body as Bullet;
					HandleBulletSpawn(bullet);
					break;
				}
				case NotificationEnum.BULLET_DESPAWN:
				{
					Bullet bullet = notification.Body as Bullet;
					HandleBulletDespawn(bullet);
					break;
				}
				case NotificationEnum.BATTLE_PAUSE:
				{
					bool isPause = (bool)notification.Body;
					HandleBattlePause(isPause);
					break;
				}
			}
		}

		private void HandleBulletSpawn(Bullet bullet)
		{
			bulletProxy.AddBullet(bullet);
		}
		private void HandleBulletDespawn(Bullet bullet)
		{
			bulletProxy.RemoveBullet(bullet.Uid);
		}
		private void HandleBattlePause(bool isPause)
		{
			bulletProxy.IterateMonsters((Bullet bullet) =>
			{
				bullet.Pause(isPause);
			});
		}
    }

}

