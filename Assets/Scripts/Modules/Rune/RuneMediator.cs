using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
    public enum RuneType
    {
        None,
        Life,
        Buff,
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
                NotificationEnum.PACK_USE_ITEM,
            };
        }
        public override void HandleNotification(INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.PACK_USE_ITEM:
                    int kid = (int)notification.Body;
                    HandleUseRune(kid);
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

            RuneType type = RuneProxy.GetRuneType(itemData);
            switch(type)
            {
                case RuneType.Life:
                    {
                        float param2 = !string.IsNullOrEmpty(itemData.Param2) ? float.Parse(itemData.Param2) : 0;
                        battleProxy.DoHealHero((int)param2);
                        break;
                    }
                case RuneType.Buff:
                    {
                        int buffKid = int.Parse(itemData.Param2);
                        Adam.Instance.AddBuff(buffKid);
                        break;
                    }
            }
        }
    }
}

