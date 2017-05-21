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

		private HeroProxy heroProxy;

        public override void OnRegister()
        {
			heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
        }

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.BATTLE_UI_INIT,
				NotificationEnum.BATTLE_UI_UPDATE_HP,
				NotificationEnum.BATTLE_UI_UPDATE_MP,
                NotificationEnum.BATTLE_PAUSE,
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
				case NotificationEnum.BATTLE_UI_UPDATE_MP:
				{
					int value = (int)notification.Body;
					HandleUpdateMP(value);
					break;
				}
                case NotificationEnum.BATTLE_PAUSE:
                {
                    HandlePause();
                    break;
                }
			}
		}

		private void HandleUIInit()
        {
            List<HeroInfo> infoList = heroProxy.GetUnlockedHeroInfoList();

            panel = PopupManager.Instance.CreateAndAddPopup<BattleUIPanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
			panel.SetData(infoList);
            panel.CallbackHeroItemClick = OnHeroItemClick;
            panel.CallbackProfileClick = OnProfileClicked;
            EventTriggerListener.Get(panel.ButtonPause.gameObject).onClick = OnPauseGame;
            EventTriggerListener.Get(panel.ButtonPack.gameObject).onClick = OnShowPack;

            float hpVal = Adam.Instance.Info.HP * 1f / Adam.Instance.Data.HP;
            panel.UpdateLifeBar(hpVal, false);
		}
		private void HandleUpdateHP(AttackResult ar)
		{
			panel.UpdateHPNumber(ar);
            float hpVal = Adam.Instance.Info.HP * 1f / Adam.Instance.Data.HP;
            panel.UpdateLifeBar(hpVal, true);
		}
		private void HandleUpdateMP(int value)
		{
            panel.UpdateMagicBar(value);
		}
        private void HandlePause()
        {
            panel.ShowHeroItems(!Game.Instance.IsPause);
        }

		private void OnHeroItemClick()
		{
			HeroData data = panel.CurrentItem.Data;
			DispatchNotification(NotificationEnum.HERO_CONVERT, data.Kid);
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

