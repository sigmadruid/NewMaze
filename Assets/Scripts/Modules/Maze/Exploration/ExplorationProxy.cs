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

        private Dictionary<int, List<ExplorationRecord>> recordDic = new Dictionary<int, List<ExplorationRecord>>();
        private Dictionary<string, Exploration> explorationDic = new Dictionary<string, Exploration>();

        public HashSet<Exploration> enteredExplorationSet = new HashSet<Exploration>();
		
        public Dictionary<int, List<ExplorationRecord>> RecordDic
        {
            get { return recordDic; }
            set { recordDic = value; }
        }

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

        public void InitRecordList(Block block)
        {
            if(block.IsRoom)
            {
                block.ForeachNode((MazePosition mazePos) =>
                    {
                        int location = Maze.GetCurrentLocation(mazePos.Col, mazePos.Row);
                        recordDic[location] = new List<ExplorationRecord>();
                    });
            }
            else
            {
                int location = Maze.GetCurrentLocation(block.Col, block.Row);
                recordDic[location] = new List<ExplorationRecord>();
            }
        }
        public void InitRecordList(Hall hall)
        {
            int location = Maze.GetCurrentLocation(hall.Data.Kid);
            recordDic[location] = new List<ExplorationRecord>();
        }
        public List<ExplorationRecord> GetRecordList(int location)
        {
            List<ExplorationRecord> recordList = null;
            recordDic.TryGetValue(location, out recordList);
            return recordList;
        }
        public Dictionary<int, List<ExplorationRecord>> GetRecord()
        {
            int location;
            if(Hall.Instance != null)
            {
                InitRecordList(Hall.Instance);
            }
            else
            {
                BlockProxy blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
                blockProxy.Iterate((Block block) =>
                    {
                        InitRecordList(block);
                    });
            }
            var enumerator = explorationDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Exploration expl = enumerator.Current.Value;
                location = Maze.GetCurrentLocation(expl.WorldPosition);
                List<ExplorationRecord> recordList = GetRecordList(location);
                recordList.Add(expl.ToRecord());
            }
            return recordDic;
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

        public void Claim(Exploration expl)
        {
//            if(!recordDic.ContainsKey(expl.Uid))
//            {
//                recordDic.Add(expl.Uid, expl.ToRecord());
//            }
        }
    }
}

