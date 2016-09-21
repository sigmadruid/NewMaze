using System;

using Base;

namespace GameLogic
{
    public class PackMediator : Mediator
    {
        private PackProxy packProxy;

        public override void OnRegister()
        {
            base.OnRegister();
            packProxy = ApplicationFacade.Instance.RetrieveProxy<PackProxy>();
        }

        public override System.Collections.Generic.IList<Enum> ListNotificationInterests()
        {
            return new Enum[]
            {
                NotificationEnum.PACK_SHOW,
                NotificationEnum.PACK_REFRESH,
            };
        }

        public override void HandleNotification(INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.PACK_SHOW:
                    break;
                case NotificationEnum.PACK_REFRESH:
                    break;
            }
        }
    }
}