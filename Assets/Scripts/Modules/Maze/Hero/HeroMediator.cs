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
        private MazePosition currentMazePos;
        private ParticleScript convertEffect;

        private PlayerProxy playerProxy;
        private HeroProxy heroProxy;
        private BlockProxy blockProxy;
		
		public override void OnRegister ()
		{
            playerProxy = ApplicationFacade.Instance.RetrieveProxy<PlayerProxy>();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.HERO_INIT,
                NotificationEnum.MOUSE_HIT_OBJECT,
                NotificationEnum.HERO_CONVERT_START,
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
				case NotificationEnum.HERO_CONVERT_START:
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
            PlayerInfo playerInfo = playerProxy.CurrentInfo;
            HeroInfo info = heroProxy.GetInfoByKid(playerInfo.HeroKid);

            adam = Adam.Create(info);
            adam.SetPosition(playerInfo.StartPosition);
            adam.SetRotation(playerInfo.StartAngle);
			adam.CallbackDie = OnHeroDie;
            adam.CallbackHeartBeat = OnHeartBeat;

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
            if (!adam.Info.IsAlive || playerProxy.CurrentInfo.IsConverting)
			{
				return;
			}

			convertEffect.ResetTask();
            convertEffect.AddTask(1f, OnConversionMiddle, heroKid);
			convertEffect.AddTask(2f, OnConversionFinished);
            convertEffect.Active(adam.WorldPosition);

			InputManager.Instance.Enable = false;

            playerProxy.CurrentInfo.IsConverting = true;
            adam.Exit();
		}

		private void HandleBattlePause(bool isPause)
		{
            if(Game.Instance.CurrentStageType == StageEnum.HomeTown)
                return;
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
        private void OnHeartBeat()
        {
            if(playerProxy.CurrentInfo.IsInHall)
                return;
                
            MazePosition pos = adam.GetMazePosition();
            if(currentMazePos != pos)
            {
                currentMazePos = pos;
                blockProxy.AddMockNode(currentMazePos);
            }
        }
        private void OnConversionMiddle(object param)
        {
            int heroKid = (int)param;

            Vector3 position = adam.WorldPosition;
            float angle = adam.WorldAngle;

            Adam.Recycle();
            HeroInfo info = heroProxy.GetInfoByKid(heroKid);
            adam = Adam.Create(info);
            adam.SetPosition(position);
            adam.SetRotation(angle);
            adam.CallbackDie = OnHeroDie;
            adam.CallbackHeartBeat = OnHeartBeat;

            BattleProxy battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
            battleProxy.SetAdam(adam);
        }
		private void OnConversionFinished(object param)
		{
			InputManager.Instance.Enable = true;
			convertEffect.Deactive();
			
            playerProxy.CurrentInfo.IsConverting = false;
            DispatchNotification(NotificationEnum.HERO_CONVERT_END);
		}

		#endregion
	}
}

