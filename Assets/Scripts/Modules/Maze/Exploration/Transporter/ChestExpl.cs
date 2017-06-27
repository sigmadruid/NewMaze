using UnityEngine;

using System;

using Base;
using StaticData;

namespace GameLogic
{
    public class ChestExpl : Exploration
    {
        public override void OnFunction()
        {
            base.OnFunction();

            Script.GetComponent<Collider>().enabled = false;

            Tupple<int, Vector3> tupple = new Tupple<int, Vector3>();
            tupple.Item1 = int.Parse(Data.Param1);
            tupple.Item2 = WorldPosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DROP_CREATED, tupple);
        }

        public static void PostCreate(Exploration expl)
        {

        }
    }
}

