using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
    public class ExplorationProxy : Proxy
    {
        private Dictionary<string, Exploration> explorationBlockDic = new Dictionary<string, Exploration>();
        private Dictionary<string, Exploration> explorationHallDic = new Dictionary<string, Exploration>();

        public Dictionary<int, List<ExplorationRecord>> RecordDic = new Dictionary<int, List<ExplorationRecord>>();

        public HashSet<Exploration> enteredExplorationSet = new HashSet<Exploration>();
		
        public void Init()
        {
        }
        public void Dispose()
        {
            Dictionary<string, Exploration> explorationDic = GetCurrentDic();
            foreach(Exploration exploration in explorationDic.Values)
            {
                Exploration.Recycle(exploration);
            }
            explorationDic.Clear();

            enteredExplorationSet.Clear();
        }
        public void SaveRecord()
        {
            int hallKid = Hall.IsActive ? Hall.Instance.Data.Kid : 0;
            Dictionary<string, Exploration> explorationDic = GetCurrentDic();

            List<ExplorationRecord> recordList = null;
            if(RecordDic.ContainsKey(hallKid))
            {
                recordList = RecordDic[hallKid];
                recordList.Clear();
            }
            else
            {
                recordList = new List<ExplorationRecord>();
                RecordDic[hallKid] = recordList;
            }
            foreach(Exploration exploration in explorationDic.Values)
            {
                recordList.Add(exploration.ToRecord());
            }
        }

        #region Add and Remove

        public void ForeachBlockExpl(Action<Exploration> action)
        {
            if (action == null) { return; }

            var enumerator = explorationBlockDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }
        public void ForeachHallExpl(Action<Exploration> action)
        {
            if (action == null) { return; }

            var enumerator = explorationHallDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }

		public void AddExpl(Exploration exploration)
		{
            Dictionary<string, Exploration> explorationDic = GetCurrentDic();
            if (!explorationDic.ContainsKey(exploration.Uid))
			{
                explorationDic.Add(exploration.Uid, exploration);
			}
		}
		public void RemoveExpl(string uid)
		{
            Dictionary<string, Exploration> explorationDic = GetCurrentDic();
            if (explorationDic.ContainsKey(uid))
			{
                Exploration expl = explorationBlockDic[uid];
                explorationBlockDic.Remove(uid);

                RemoveEnteredExploration(expl);
			}
		}

        #endregion

        #region Enter and Exit
		
        public Exploration FindNearbyExploration(Vector3 position)
        {
            foreach(Exploration expl in enteredExplorationSet)
            {
                if (Vector3.SqrMagnitude(expl.WorldPosition - position) < GlobalConfig.ExplorationConfig.NearSqrDistance)
                {
                    return expl;
                }
            }
            return null;
        }
        public void AddEnteredExploration(Exploration expl)
        {
            if (!enteredExplorationSet.Contains(expl))
                enteredExplorationSet.Add(expl);
        }
        public void RemoveEnteredExploration(Exploration expl)
        {
            if (enteredExplorationSet.Contains(expl))
                enteredExplorationSet.Remove(expl);
        }
        public void ForeachEntered(Action<Exploration> action)
        {
            if (action == null) { return; }

            var enumerator = enteredExplorationSet.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        #endregion

        public void Claim(Exploration expl)
        {
            RemoveExpl(expl.Uid);
            Exploration.Recycle(expl);
        }

        private Dictionary<string, Exploration> GetCurrentDic()
        {
            return Hall.IsActive ? explorationHallDic : explorationBlockDic;
        }
    }
}

