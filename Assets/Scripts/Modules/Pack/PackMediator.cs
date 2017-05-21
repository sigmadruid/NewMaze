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
            packPanel = PopupManager.Instance.CreateAndAddPopup<PackPanel>(PopupMode.SHOW | PopupMode.ADD_MASK);
            packPanel.CallbackSwitchType = OnSwitch;
            packPanel.CallbackUseItem = OnUse;
            packPanel.CallbackDiscardItem = OnDiscard;
            List<ItemInfo> itemInfoList = packProxy.GetItemInfosByType(ItemType.Resource);
            packPanel.SetInfo(ItemType.Resource, itemInfoList);
        }
        private void HandlePackRefresh(ItemType type)
        {
            if (type == ItemType.None)
            {
                type = packPanel.CurrentItemType;
            }
            List<ItemInfo> itemInfoList = packProxy.GetItemInfosByType(type);
            packPanel.SetInfo(type, itemInfoList);
        }

        private void OnSwitch(ItemType itemType)
        {
        }
        private void OnUse(ItemInfo itemInfo)
        {
        }
        private void OnDiscard(ItemInfo itemInfo)
        {
        }

    }
}