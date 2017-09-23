using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using Battle;
using StaticData;

namespace GameLogic
{
	public class TownHeroMediator : Mediator
	{
        private Adam adam;

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
            int heroKid = IDManager.Instance.GetKid(IDType.Hero, 1);
            HeroData data = HeroDataManager.Instance.GetData(heroKid) as HeroData;
            HeroInfo info = new HeroInfo(data);

            adam = Adam.Create(info);
            adam.SetPosition(new Vector3(-1.77f, 0, 13.02f));
		}
		private void HandleTownHeroDispose()
		{
            Adam.Recycle();
		}

//		private void OnUpdate()
//		{
//            float speed = GlobalConfig.HeroConfig.TownAdamWalkSpeed;
//            if(inputManager.MouseHitPosition != Vector3.zero)
//            {
//                if(MathUtils.XZSqrDistance(heroScript.transform.position, inputManager.MouseHitPosition) > GlobalConfig.InputConfig.NearSqrDistance)
//                {
//                    heroScript.Move(inputManager.MouseHitPosition, speed);
//                }
//                else
//                {
//                    heroScript.Move(Vector3.zero, 0);
//                }
//            }
//            else
//            {
//                heroScript.Move(Vector3.zero, 0);
//            }
//
//		}
	}
}

