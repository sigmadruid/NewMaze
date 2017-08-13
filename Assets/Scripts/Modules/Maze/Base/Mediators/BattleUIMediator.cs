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

        private PackProxy packProxy;

        public override void OnRegister()
        {
            packProxy = ApplicationFacade.Instance.RetrieveProxy<PackProxy>();
        }

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.BATTLE_UI_INIT,
				NotificationEnum.BATTLE_UI_UPDATE_HP,
                NotificationEnum.BATTLE_PAUSE,
                NotificationEnum.HERO_CONVERT_END,
                NotificationEnum.PACK_USE_ITEM,
                NotificationEnum.PACK_CHANGE_ITEM_COUNT,
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
                case NotificationEnum.PACK_USE_ITEM:
                    {
                        int kid = (int)notification.Body;
                        HandleUpdateItem(kid);
                        break;
                    }
                case NotificationEnum.PACK_CHANGE_ITEM_COUNT:
                    {
                        int kid = (int)notification.Body;
                        HandleUpdateItem(kid);
                        break;
                    }
			}
		}

        #region Event Handlers

		private void HandleUIInit()
        {

            panel = PopupManager.Instance.CreateAndAddPopup<BattleUIPanel>();
            panel.CallbackHeroItemClick = OnHeroItemClick;
            panel.CallbackRuneItemClick = OnRuneItemClick;
            panel.CallbackProfileClick = OnProfileClicked;
            ClickEventTrigger.Get(panel.ButtonPause.gameObject).onClick = OnPauseGame;
            ClickEventTrigger.Get(panel.ButtonPack.gameObject).onClick = OnShowPack;

            List<int> kidList = GlobalConfig.DemoConfig.InitialHeroKids;
            List<ItemInfo> runeInfoList = packProxy.GetItemInfosByType(ItemType.Rune);
            panel.SetHeroListData(kidList, runeInfoList);
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
        private void HandleUpdateItem(int kid)
        {
            if(panel == null)
                return;
            ItemData data = ItemDataManager.Instance.GetData(kid) as ItemData;
            if(data.Type != ItemType.Rune)
                return;
            List<ItemInfo> runeInfoList = packProxy.GetItemInfosByType(ItemType.Rune);
            panel.SetRuneData(runeInfoList);
        }

        #endregion

        #region UI Event Listeners

        private void OnHeroItemClick(int kid)
		{
            HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
			DispatchNotification(NotificationEnum.HERO_CONVERT_START, data.Kid);
		}
        private void OnRuneItemClick(int kid)
        {
            packProxy.Use(kid);
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

        #endregion
    }
}

