using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class HeroMediator : Mediator
	{
		private Hero hero;

		private bool isConverting;

		private EffectScript convertEffect;

		private HeroProxy heroProxy;
		
		public override void OnRegister ()
		{
			heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.HERO_INIT,
				NotificationEnum.HERO_CONVERT,
				NotificationEnum.BATTLE_PAUSE,
				NotificationEnum.HERO_TRANSPORT,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.HERO_INIT:
				{
					HandleHeroInit();
					break;
				}
				case NotificationEnum.HERO_CONVERT:
				{
					int heroKid = (int)notification.Body;
					HandleHeroConvert(heroKid);
					break;
				}
				case NotificationEnum.BATTLE_PAUSE:
				{
					bool isPause = (bool)notification.Body;
					HandleBattlePause(isPause);
					break;
				}
				case NotificationEnum.HERO_TRANSPORT:
				{
					Vector3 destPosition = (Vector3)notification.Body;
					HandleHeroTransport(destPosition);
					break;
				}
			}
		}

		#region Notification Handlers

		private void HandleHeroInit()
		{
			//Preload resources
			List<HeroData> dataList = heroProxy.GetAllHeroDataList();
			for (int i = 0; i < dataList.Count; ++i)
			{
				HeroData heroData = dataList[i];
				ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, heroData.GetResPath());
			}
			ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, "Effects/ConversionEffect");

			//Init
			int heroID = IDManager.Instance.GetID(IDType.Hero, 1);
			hero = Hero.Create(heroID, null);
			hero.CallbackDie = OnHeroDie;

			//Position
			MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
			Vector3 startPosition = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);
			hero.SetPosition(startPosition);

			//Battle
			BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			battleProxy.SetHero(hero);

			//Conversion
			convertEffect = ResourceManager.Instance.LoadAsset<EffectScript>(ObjectType.GameObject, "Effects/ConversionEffect");
			convertEffect.Deactive();

			//Input
			InputManager.Instance.CallbackFire = OnHeroAttack;
			InputManager.Instance.CallbackFunction = OnHeroFunction;

//            hero.Info.AddHP(-950);
		}

		private void HandleHeroConvert(int heroKid)
		{
			if (!hero.Info.IsAlive || hero.Info.IsConverting)
			{
				return;
			}
			hero.Info.IsConverting = true;

			convertEffect.ResetTask();
			convertEffect.AddTask(1f, OnConversionMiddle, heroKid);
			convertEffect.AddTask(2f, OnConversionFinished);
			convertEffect.Active(hero.WorldPosition);

			InputManager.Instance.Enable = false;
		}

		private void HandleBattlePause(bool isPause)
		{
			if (convertEffect != null)
			{
				convertEffect.IsEnabled = !isPause;
			}
			hero.Pause(isPause);
		}

		private void HandleHeroTransport(Vector3 destPosition)
		{
			hero.SetPosition(destPosition);
		}

		#endregion
		
		#region Event Handlers

		private void OnHeroAttack()
		{
			if (hero != null)
				hero.Attack();
		}
		private void OnHeroFunction()
		{
			if (hero != null)
			{
				hero.Function();
                DispatchNotification(NotificationEnum.DROP_PICKED_UP);
                DispatchNotification(NotificationEnum.EXPLORATION_FUNCTION);
			}
		}

		private void OnHeroDie()
		{
			Game.Instance.SwitchStage(StageEnum.HomeTown);
		}

		private void OnConversionMiddle(object param)
		{
			int heroKid = (int)param;
			
			Vector3 position = hero.WorldPosition;
			float angle = hero.Script.transform.localEulerAngles.y;
			
			Hero.Recycle();
			Hero newHero = Hero.Create(heroKid, hero.Info);
			newHero.SetPosition(position);
			newHero.SetRotation(angle);
			hero = newHero;
			hero.CallbackDie = OnHeroDie;
			
			BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			battleProxy.SetHero(newHero);
		}
		private void OnConversionFinished(object param)
		{
			InputManager.Instance.Enable = true;
			convertEffect.Deactive();
			
			hero.Info.IsConverting = false;
		}

		#endregion
	}
}

