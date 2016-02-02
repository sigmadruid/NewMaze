using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace GameLogic
{
    public class ExplorationProxy : Proxy
    {
		private Dictionary<string, Exploration> explorationDic = new Dictionary<string, Exploration>();
		
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
    }
}

