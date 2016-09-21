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

        private Dictionary<string, Exploration> explorationBlockDic = new Dictionary<string, Exploration>();
        private Dictionary<string, Exploration> explorationHallDic = new Dictionary<string, Exploration>();
		

        public void IterateInBlocks(IterateFunc func)
        {
            if (func == null) { return; }

            Dictionary<string, Exploration>.Enumerator enumerator = explorationBlockDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
        }

		public void AddInBlock(Exploration exploration)
		{
			if (!explorationBlockDic.ContainsKey(exploration.Uid))
			{
				explorationBlockDic.Add(exploration.Uid, exploration);
			}
		}
		public void RemoveInBlock(string uid)
		{
			if (explorationBlockDic.ContainsKey(uid))
			{
				explorationBlockDic.Remove(uid);
			}
		}
        public void ClearInBlocks()
        {
            Dictionary<string, Exploration>.Enumerator blockEnum = explorationBlockDic.GetEnumerator();
            while(blockEnum.MoveNext())
            {
                Exploration.Recycle(blockEnum.Current.Value);
            }
            explorationBlockDic.Clear();
        }

        public void IterateInHall(IterateFunc func)
        {
            if (func == null) { return; }

            Dictionary<string, Exploration>.Enumerator enumerator = explorationHallDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
        }

        public void AddInHall(Exploration exploration)
        {
            if (!explorationHallDic.ContainsKey(exploration.Uid))
            {
                explorationHallDic.Add(exploration.Uid, exploration);
            }
        }
        public void RemoveInHall(string uid)
        {
            if (explorationHallDic.ContainsKey(uid))
            {
                Exploration expl = explorationHallDic[uid];
                RemoveEnteredExploration(expl);
            }
        }
        public void ClearInHall()
        {
            Dictionary<string, Exploration>.Enumerator hallEnum = explorationHallDic.GetEnumerator();
            while(hallEnum.MoveNext())
            {
                Exploration.Recycle(hallEnum.Current.Value);
            }
            explorationHallDic.Clear();
        }
		
		public void Dispose()
		{
            ClearInBlocks();
            ClearInHall();

            enteredExplorationSet.Clear();
		}

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
    }
}

