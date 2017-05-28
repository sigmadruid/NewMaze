using System;

using Base;

namespace GameLogic
{
    public class AdamProxy : Proxy
    {
        public AdamRecord AdamRecord;

        public void DoRecord()
        {
            AdamRecord = Adam.Instance.ToRecord();
        }
    }
}

