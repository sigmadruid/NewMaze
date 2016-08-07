using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class TownHeroMediator : Mediator
	{
		private TownHeroScript heroScript;

		private InputManager inputManager;

		public override void OnRegister ()
		{
			inputManager = InputManager.Instance;
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.TOWN_HERO_INIT,
				NotificationEnum.TOWN_HERO_DISPOSE,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.TOWN_HERO_INIT:
					HandleTownHeroInit();
					break;
				case NotificationEnum.TOWN_HERO_DISPOSE:
					HandleTownHeroDispose();
					break;
			}
		}

		private void HandleTownHeroInit()
		{
			heroScript = GameObject.Find("TownHero").GetComponent<TownHeroScript>();
			heroScript.CallbackUpdate = OnUpdate;
		}
		private void HandleTownHeroDispose()
		{
			GameObject.Destroy(heroScript.gameObject);
		}

		private void OnUpdate()
		{
            if(inputManager.MouseHitPosition != Vector3.zero)
            {
                if(MathUtils.XZSqrDistance(heroScript.transform.position, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
                {
                    Vector3 direction = MathUtils.XZDirection(heroScript.transform.position, inputManager.MouseHitPosition);
                    heroScript.Move(direction, 3f);
                }
            }

		}
	}
}

