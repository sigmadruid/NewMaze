using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using GameUI;
using Battle;
using Base;
using StaticData;

namespace GameLogic
{
    public class BattleUIMediator : Mediator
    {
		private BattleUIPanel panel;

        public override void OnRegister()
        {
        }

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.BATTLE_UI_INIT,
				NotificationEnum.BATTLE_UI_UPDATE_HP,
                NotificationEnum.BATTLE_PAUSE,
                NotificationEnum.HERO_CONVERT_END,
			};
		}

		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.BATTLE_UI_INIT:
    				{
                        HandleUIInit();
                        break;
                    }
				case NotificationEnum.BATTLE_UI_UPDATE_HP:
    				{
    					AttackResult ar = (AttackResult)notification.Body;
    					HandleUpdateHP(ar);
    					break;
    				}
                case NotificationEnum.BATTLE_PAUSE:
                    {
                        HandlePause();
                        break;
                    }
                case NotificationEnum.HERO_CONVERT_END:
                    {
                        HandleHeroConvertEnd();
                        break;
                    }
			}
		}

		private void HandleUIInit()
        {
            List<int> kidList = GlobalConfig.DemoConfig.InitialHeroKids;

            panel = PopupManager.Instance.CreateAndAddPopup<BattleUIPanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
            panel.SetHeroListData(kidList);
            panel.CallbackHeroItemClick = OnHeroItemClick;
            panel.CallbackProfileClick = OnProfileClicked;
            ClickEventTrigger.Get(panel.ButtonPause.gameObject).onClick = OnPauseGame;
            ClickEventTrigger.Get(panel.ButtonPack.gameObject).onClick = OnShowPack;

            panel.UpdateHPBar(Adam.Instance.Info.HPRatio, false);
		}
		private void HandleUpdateHP(AttackResult ar)
		{
			panel.UpdateHPNumber(ar);
            panel.UpdateHPBar(Adam.Instance.Info.HPRatio, true);
		}
        private void HandlePause()
        {
        }
        private void HandleHeroConvertEnd()
        {
            panel.SetHeroData();   
        }

		private void OnHeroItemClick()
		{
			HeroData data = panel.CurrentItem.Data;
			DispatchNotification(NotificationEnum.HERO_CONVERT_START, data.Kid);
		}
        private void OnProfileClicked()
        {
            DispatchNotification(NotificationEnum.PROFILE_SHOW);
        }
        private void OnPauseGame(GameObject go)
		{
            PopupManager.Instance.CreateAndAddPopup<PausePanel>();
            Game.Instance.SetPause(true);
		}
        private void OnShowPack(GameObject go)
        {
            DispatchNotification(NotificationEnum.PACK_SHOW, true);
        }
    }
}

