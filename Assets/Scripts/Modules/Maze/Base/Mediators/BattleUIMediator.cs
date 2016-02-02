using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class BattleUIMediator : Mediator
    {
		private const string PANEL_PATH = "UI/BattleUIPanel";

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
			}
		}

		private void HandleUIInit()
        {
			List<HeroData> dataList = heroProxy.GetAllHeroDataList();

			panel = PopupManager.Instance.CreateAndAddPopup<BattleUIPanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
			panel.Init(dataList);
			panel.CallbackItemClick = OnItemClick;
			panel.CallbackUpdate = OnUpdate;
			UIEventListener.Get(panel.ButtonShowMap.gameObject).onClick = OnShowMap;
			UIEventListener.Get(panel.ButtonPause.gameObject).onClick = OnPauseGame;
		}
		private void HandleUpdateHP(AttackResult ar)
		{
			panel.UpdateHPNumber(ar);
		}
		private void HandleUpdateMP(int value)
		{
			panel.UpdateMPNumber(value);
		}

		private void OnItemClick()
		{
			HeroData data = panel.CurrentItem.Data;
			DispatchNotification(NotificationEnum.HERO_CONVERT, data.Kid);
		}
		private void OnUpdate()
		{
			float hpVal = Hero.Instance.Info.HP * 1f / Hero.Instance.Data.HP;
			float mpVal = 1f;
			panel.UpdateSliderBar(hpVal, mpVal);
		}
		private void OnShowMap(GameObject go)
		{
			DispatchNotification(NotificationEnum.MAZE_MAP_SHOW, true);
		}
		private void OnPauseGame(GameObject go)
		{
			Game.Instance.SetPause();
			DispatchNotification(NotificationEnum.BATTLE_PAUSE, Game.Instance.IsPause);

			panel.ShowHeroItems(!Game.Instance.IsPause);
		}
    }
}

