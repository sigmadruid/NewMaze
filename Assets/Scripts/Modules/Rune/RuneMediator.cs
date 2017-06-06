using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
    public enum RuneType
    {
        Life,
        Attack,
        Defense,
        Dexterity,
        Evasion,
        Speed,
        Invisible,
    }
    public class RuneMediator : Mediator
    {
        private BattleProxy battleProxy;

        public override void OnRegister()
        {
            battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
        }

        public override IList<Enum> ListNotificationInterests()
        {
            return new Enum[]
            {
                NotificationEnum.USE_ITEM,
            };
        }
        public override void HandleNotification(INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.USE_ITEM:
                    HandleUseRune((int)notification.Body);
                    break;
            }
        }

        private void HandleUseRune(int kid)
        {
            ItemData itemData = ItemDataManager.Instance.GetData(kid) as ItemData;
            if(itemData == null)
            {
                BaseLogger.LogError("Can't find rune: kid=" + kid.ToString());
                return;
            }
            if (itemData.Type != ItemType.Rune)
            {
                BaseLogger.LogError("This is not a rune: kid=" + kid.ToString());
                return;
            }

            RuneType type = (RuneType)Enum.Parse(typeof(RuneType), itemData.Param1);
            float param2 = !string.IsNullOrEmpty(itemData.Param2) ? float.Parse(itemData.Param2) : 0;
            float param3 = !string.IsNullOrEmpty(itemData.Param3) ? float.Parse(itemData.Param3) : 0;
            switch(type)
            {
                case RuneType.Life:
                    {
                        battleProxy.DoHealHero((int)param2);
                        break;
                    }
            }
        }
    }
}

