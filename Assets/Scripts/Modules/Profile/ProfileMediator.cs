using System;
using System.Collections.Generic;

using Base;
using Battle;
using GameUI;

namespace GameLogic
{
    public class ProfileMediator : Mediator
    {
        private ProfilePanel profilePanel;
        private HeroProxy heroProxy;

        public override void OnRegister()
        {
            base.OnRegister();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
        }

        public override IList<Enum> ListNotificationInterests()
        {
            return new Enum[]
            {
                NotificationEnum.PROFILE_SHOW,
            };
        }

        public override void HandleNotification(INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.PROFILE_SHOW:
                    HandleProfileShow();
                    break;
            }
        }
        private void HandleProfileShow()
        {
            List<HeroInfo> infoList = heroProxy.GetUnlockedHeroInfoList();

            profilePanel = PopupManager.Instance.CreateAndAddPopup<ProfilePanel>(PopupMode.SHOW | PopupMode.ADD_MASK);
            profilePanel.SetData(infoList);
        }
    }
}

