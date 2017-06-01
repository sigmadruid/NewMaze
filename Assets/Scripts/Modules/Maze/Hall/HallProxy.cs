using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
    public class HallProxy : Proxy
    {
        public HallRecord Record;

        public void Init()
        {
        }
        public void Dispose()
        {
            Record = null;
        }

        public HallRecord CreateRecord()
        {
            if(Hall.Instance != null)
                Record = Hall.Instance.ToRecord();
            return Record;
        }
        public void SetRecord(HallRecord record)
        {
            Record = record;
        }
    }
}

