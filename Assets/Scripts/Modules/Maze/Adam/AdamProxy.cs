using System;

using Base;

namespace GameLogic
{
    public class AdamProxy : Proxy
    {
        public AdamRecord AdamRecord;

        public AdamRecord CreateRecord()
        {
            var facade = ApplicationFacade.Instance;

            AdamRecord = Adam.Instance.ToRecord();
            AdamRecord.PackItems = facade.RetrieveProxy<PackProxy>().GetRecord();
            return AdamRecord;
        }
        public void SetRecord(AdamRecord record)
        {
            var facade = ApplicationFacade.Instance;

            AdamRecord = record;
            facade.RetrieveProxy<PackProxy>().SetRecord(AdamRecord.PackItems);
        }
    }
}

