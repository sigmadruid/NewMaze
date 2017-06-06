using System;

using Base;

namespace GameLogic
{
    public class AdamProxy : Proxy
    {
        public AdamRecord AdamRecord;

        public bool IsConverting;
        public bool IsInHall;
        public bool IsVisible;

        public AdamRecord CreateRecord()
        {
            var facade = ApplicationFacade.Instance;

            AdamRecord = Adam.Instance.ToRecord();
            AdamRecord.IsInHall = IsInHall;
            AdamRecord.IsVisible = IsVisible;
            AdamRecord.PackItems = facade.RetrieveProxy<PackProxy>().GetRecord();
            return AdamRecord;
        }
        public void SetRecord(AdamRecord record)
        {
            var facade = ApplicationFacade.Instance;

            AdamRecord = record;
            IsInHall = AdamRecord.IsInHall;
            IsVisible = AdamRecord.IsVisible;
            facade.RetrieveProxy<PackProxy>().SetRecord(AdamRecord.PackItems);
        }
    }
}

