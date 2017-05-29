using System;

using Base;

namespace GameLogic
{
    public class AdamProxy : Proxy
    {
        public AdamRecord AdamRecord;

        public AdamRecord GetRecord()
        {
            return Adam.Instance.ToRecord();
        }
    }
}

