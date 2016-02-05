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
        public HashSet<Exploration> enteredExplorationSet = new HashSet<Exploration>();

		private Dictionary<string, Exploration> explorationDic = new Dictionary<string, Exploration>();
		
        public Exploration CreateExploration(ExplorationType type, List<object> paramList = null)
        {
            Exploration exploration = null;
            switch (type)
            {
                case ExplorationType.Transporter:   
                    exploration = new TransporterExpl();
                    TransporterDirectionType directionType = (TransporterDirectionType)paramList[0];
                    TransporterExpl.Init(exploration as TransporterExpl, type);
                    break;
                default:                           
                    exploration = new Exploration();
                    Exploration.Init(exploration, type);
                    break;
            }
            return exploration;
        }

		public List<Exploration> GetAll()
		{
			return explorationDic.Values.ToList();
		}
		
		public void Add(Exploration exploration)
		{
			if (!explorationDic.ContainsKey(exploration.Uid))
			{
				explorationDic.Add(exploration.Uid, exploration);
			}
		}
		public void Remove(string uid)
		{
			if (explorationDic.ContainsKey(uid))
			{
				explorationDic.Remove(uid);
			}
		}
		
		public void Dispose()
		{
			List<Exploration> list = explorationDic.Values.ToList();
			int count = list.Count;
			for (int i = 0; i < count; ++i)
			{
				Exploration.Recycle(list[i]);
			}
			explorationDic.Clear();
		}

        public Exploration FindNearbyExploration(Vector3 position)
        {
            foreach(Exploration expl in enteredExplorationSet)
            {
                if (Vector3.SqrMagnitude(expl.WorldPosition - position) < GlobalConfig.InputConfig.NearSqrDistance)
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

