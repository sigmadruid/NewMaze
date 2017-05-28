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
        private Adam adam;

		private bool isConverting;

        private ParticleScript convertEffect;

        private AdamProxy adamProxy;
		private HeroProxy heroProxy;
		
		public override void OnRegister ()
		{
            adamProxy = ApplicationFacade.Instance.RetrieveProxy<AdamProxy>();
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
                    HandleHeroInit();
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

        private void HandleHeroInit()
		{
			//Init
            int heroKid = IDManager.Instance.GetKid(IDType.Hero, 1);
            HeroInfo info = heroProxy.GetHeroInfo(heroKid);
            adam = Adam.Create(info);

            AdamRecord record = adamProxy.AdamRecord;
            if(record != null)
            {
                adam.SetPosition(record.WorldPosition.ToVector3());
                adam.SetRotation(record.WorldAngle);
            }
            else
            {
                MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
                Vector3 startPosition = MazeUtil.GetWorldPosition(mazeData.StartCol, mazeData.StartRow, mazeData.BlockSize);
                adam.SetPosition(startPosition);
                adam.SetRotation(0);
            }
			adam.CallbackDie = OnHeroDie;

			//Battle
			BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			battleProxy.SetAdam(adam);

			//Conversion
            convertEffect = ResourceManager.Instance.LoadAsset<ParticleScript>(ObjectType.GameObject, "Effects/ConversionEffect");
			convertEffect.Deactive();

			//Input
//            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Attack, OnHeroAttack);
//            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Function, OnHeroFunction);
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
            if (!adam.Info.IsAlive || adam.Info.IsConverting)
			{
				return;
			}

			convertEffect.ResetTask();
			convertEffect.AddTask(2f, OnConversionFinished);
            convertEffect.Active(adam.WorldPosition);

			InputManager.Instance.Enable = false;

            adam.Info.IsConverting = true;
            HeroInfo info = heroProxy.GetHeroInfo(heroKid);
            adam.Convert(info);
		}

		private void HandleBattlePause(bool isPause)
		{
			if (convertEffect != null)
			{
				convertEffect.IsEnabled = !isPause;
			}
			adam.Pause(isPause);
		}

		private void HandleHeroTransport(Vector3 destPosition)
		{
			adam.SetPosition(destPosition);
		}

		#endregion
		
		#region Event Handlers

		private void OnHeroDie()
		{
            RecordMediator.DeleteRecord();
			Game.Instance.SwitchStage(StageEnum.HomeTown);
		}

		private void OnConversionFinished(object param)
		{
			InputManager.Instance.Enable = true;
			convertEffect.Deactive();
			
			adam.Info.IsConverting = false;
		}

		#endregion
	}
}

