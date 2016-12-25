using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using Battle;
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
                NotificationEnum.MOUSE_HIT_OBJECT,
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
                    HeroRecord record = notification.Body as HeroRecord;
                    HandleHeroInit(record);
					break;
				}
                case NotificationEnum.MOUSE_HIT_OBJECT:
                {
                    HandleHeroClick();
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

        private void HandleHeroInit(HeroRecord record)
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
            if(record == null)
            {
                int heroKid = IDManager.Instance.GetKid(IDType.Hero, 1);
                hero = Hero.Create(heroKid, null);

                MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
                Vector3 startPosition = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);
                hero.SetPosition(startPosition);
            }
            else
            {
                hero = Hero.Create(record);
                hero.SetPosition(record.WorldPosition.ToVector3());
                hero.SetRotation(record.WorldAngle);
                hero.Info.IsInHall = record.IsInHall;
                hero.IsVisible = record.IsVisible;
            }
			hero.CallbackDie = OnHeroDie;

			//Battle
			BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			battleProxy.SetHero(hero);

			//Conversion
			convertEffect = ResourceManager.Instance.LoadAsset<EffectScript>(ObjectType.GameObject, "Effects/ConversionEffect");
			convertEffect.Deactive();

			//Input
//            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Attack, OnHeroAttack);
//            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Function, OnHeroFunction);

            hero.Info.AddHP(999999);
		}

        private void HandleHeroClick()
        {
            InputManager inputManager = InputManager.Instance;
            GameObject hitObject = inputManager.MouseHitObject;
            if (inputManager.CheckMouseHitLayer(Layers.LayerItem))
            {
                ItemScript itemScript = hitObject.GetComponent<ItemScript>();
                inputManager.PreventMouseAction();
                DispatchNotification(NotificationEnum.DROP_PICKED_UP, itemScript);
            }
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

		private void OnHeroDie()
		{
            RecordMediator.DeleteRecord();
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

