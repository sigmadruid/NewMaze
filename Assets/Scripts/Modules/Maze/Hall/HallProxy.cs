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

        public void CreateRecord()
        {
            if(Hall.Instance != null)
                Record = Hall.Instance.ToRecord();
        }
    }
}

