using System;

using Base;
using StaticData;

namespace GameLogic
{
    public class PackMediator : Mediator
    {
        private PackPanel packPanel;
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
                    HandlePackShow((bool)notification.Body);
                    break;
                case NotificationEnum.PACK_REFRESH:
                    HandlePackRefresh();
                    break;
            }
        }

        private void HandlePackShow(bool show)
        {
            packPanel = PopupManager.Instance.CreateAndAddPopup<PackPanel>();
            packPanel.CallbackSwitchType = OnSwitch;
            packPanel.CallbackUseItem = OnUse;
            packPanel.CallbackDeleteItem = OnDelete;
        }
        private void HandlePackRefresh()
        {
            
        }

        private void OnSwitch(ItemType itemType)
        {
        }
        private void OnUse(ItemInfo itemInfo)
        {
        }
        private void OnDelete(ItemInfo itemInfo)
        {
        }

    }
}