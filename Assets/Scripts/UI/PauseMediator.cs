using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameUI;

namespace GameLogic
{
    public class PauseMediator : Mediator
    {
        private PausePanel panel;
        private bool isShow;

        public override IList<Enum> ListNotificationInterests ()
        {
            return new Enum[]
            {
                NotificationEnum.KEY_DOWN,
            };
        }

        public override void HandleNotification (INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.KEY_DOWN:
                    {
                        KeyboardActionType actionType = (KeyboardActionType)notification.Body;
                        HandleShow(actionType);
                        break;
                    }
            }
        }

        private void HandleShow(KeyboardActionType actionType)
        {
            if(actionType != KeyboardActionType.Pause)
                return;

            if(!isShow)
            {
                panel = PopupManager.Instance.CreateAndAddPopup<PausePanel>();
                panel.CallbackPackClick = OnPackClick;
                panel.CallbackSoulAttachClick = OnSoulAttachClick;
                panel.CallbackSettingsClick = OnSettingsClick;
            }
            else
            {
                PopupManager.Instance.RemovePopup(panel);
            }
            Game.Instance.SetPause(!isShow);
            isShow = !isShow;
        }

        private void OnPackClick()
        {
            DispatchNotification(NotificationEnum.PACK_SHOW, true);
        }
        private void OnSoulAttachClick()
        {
            DispatchNotification(NotificationEnum.SOUL_ATTACH_SHOW, true);
        }
        private void OnSettingsClick()
        {
        }
    }
}

