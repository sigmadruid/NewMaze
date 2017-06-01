using System;
using System.Collections.Generic;

using Base;
using StaticData;

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
        public override void OnRegister()
        {
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
            float param1 = float.Parse(itemData.Param2);
            float param2 = float.Parse(itemData.Param3);
            switch(type)
            {
                case RuneType.Life:
                    {
                        Adam.Instance.Info.AddHP((int)param1);
                        break;
                    }
            }
        }
    }
}

