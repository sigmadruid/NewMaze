using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;

namespace GameLogic
{
    public class TriggerEntityScript : EntityScript
    {
        protected static Dictionary<string, TriggerEntityScript> entityDic = new Dictionary<string, TriggerEntityScript>();

        protected void AddSelfTrigger()
        {
            if (!entityDic.ContainsKey(Uid))
                entityDic.Add(Uid, this);
        }

        protected void RemoveSelfTrigger()
        {
            if (entityDic.ContainsKey(Uid))
                entityDic.Remove(Uid);
        }

        public static TriggerEntityScript FindNearbyExploration(Vector3 position)
        {
            foreach(TriggerEntityScript entity in entityDic.Values)
            {
                if (Vector3.SqrMagnitude(entity.transform.position - position) < GlobalConfig.ExplorationConfig.NearSqrDistance)
                {
                    return entity;
                }
            }
            return null;
        }
        public static void RemoveTrigger(string uid)
        {
            if (entityDic.ContainsKey(uid))
                entityDic.Remove(uid);
        }
        public static void ForeachTrigger(Action<TriggerEntityScript> action)
        {
            if (action == null) { return; }

            var enumerator = entityDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }
        public static void ClearTriggers()
        {
            entityDic.Clear();
        }
    }
}

