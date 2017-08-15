using System;
using System.Collections.Generic;

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

        public override IList<Enum> ListNotificationInterests()
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
                    ItemType type = (ItemType)notification.Body;
                    HandlePackRefresh(type);
                    break;
            }
        }

        private void HandlePickItem()
        {
        }
        private void HandlePackShow(bool show)
        {
            packPanel = PopupManager.Instance.CreateAndAddPopup<PackPanel>();
            packPanel.CallbackSwitchType = OnSwitch;
            packPanel.CallbackUseItem = OnUse;
            packPanel.CallbackDiscardItem = OnDiscard;
        }
        private void HandlePackRefresh(ItemType type)
        {
            if (type == ItemType.None)
            {
                type = packPanel.CurrentItemType;
            }
            packPanel.CurrentItemType = type;
        }

        private void OnSwitch(ItemType itemType)
        {
        }
        private void OnUse(ItemInfo itemInfo)
        {
            packProxy.Use(itemInfo.Data.Kid);
        }
        private void OnDiscard(ItemInfo itemInfo)
        {
        }

    }
}