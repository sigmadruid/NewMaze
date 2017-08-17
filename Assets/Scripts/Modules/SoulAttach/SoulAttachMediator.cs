using System;
using System.Collections.Generic;

using Base;

namespace GameLogic
{
    public class SoulAttachMediator : Mediator
    {
        private SoulAttachPanel panel;


        public override void OnRegister()
        {
        }
        
        public override IList<Enum> ListNotificationInterests()
        {
            return new Enum[]
            {
                NotificationEnum.SOUL_ATTACH_SHOW,
            };
        }
        public override void HandleNotification(INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.SOUL_ATTACH_SHOW:
                    HandleSoulAttachShow((bool)notification.Body);
                    break;
            }
        }

        private void HandleSoulAttachShow(bool show)
        {
            panel = PopupManager.Instance.CreateAndAddPopup<SoulAttachPanel>();
            panel.SetData();
        }
    }
}

