using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
    public class ExplorationProxy : Proxy
    {
        public delegate void IterateFunc(Exploration expl);

        public HashSet<Exploration> enteredExplorationSet = new HashSet<Exploration>();

        private Dictionary<string, Exploration> explorationDic = new Dictionary<string, Exploration>();
		
        public void Init()
        {
        }
        public void Dispose()
        {
            ClearExpls();
            enteredExplorationSet.Clear();
        }

        #region Add and Remove

        public void IterateActiveExpl(IterateFunc func)
        {
            if (func == null) { return; }

            var enumerator = explorationDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
        }

		public void AddExpl(Exploration exploration)
		{
			if (!explorationDic.ContainsKey(exploration.Uid))
			{
				explorationDic.Add(exploration.Uid, exploration);
			}
		}
		public void RemoveExpl(string uid)
		{
			if (explorationDic.ContainsKey(uid))
			{
                Exploration expl = explorationDic[uid];
                RemoveEnteredExploration(expl);
				explorationDic.Remove(uid);
			}
		}
        public void ClearExpls()
        {
            Dictionary<string, Exploration>.Enumerator blockEnum = explorationDic.GetEnumerator();
            while(blockEnum.MoveNext())
            {
                Exploration.Recycle(blockEnum.Current.Value);
            }
            explorationDic.Clear();
            enteredExplorationSet.Clear();
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
        public void IterateInEntered(IterateFunc func)
        {
            if (func == null) { return; }

            var enumerator = enteredExplorationSet.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current);
            }
        }

        #endregion
    }
}

