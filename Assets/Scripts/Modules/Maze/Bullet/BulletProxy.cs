using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
    public class BulletProxy : Proxy
    {
		public delegate void IterateFunc(Bullet bullet);
		
        private Dictionary<string, Bullet> bulletDic = new Dictionary<string, Bullet>();

        public void Init()
        {
        }
        public void Dispose()
        {
            Dictionary<string, Bullet>.Enumerator enumerator = bulletDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Bullet.Recycle(enumerator.Current.Value);
            }
            bulletDic.Clear();
        }

		public void IterateMonsters(IterateFunc func)
		{
			if (func == null) { return; }
			
			Dictionary<string, Bullet>.Enumerator enumerator = bulletDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				func(enumerator.Current.Value);
			}
		}
		
		public void AddBullet(Bullet bullet)
		{
			if (!bulletDic.ContainsKey(bullet.Uid))
			{
				bulletDic.Add(bullet.Uid, bullet);
			}
		}
		
		public void RemoveBullet(string uid)
		{
			if (bulletDic.ContainsKey(uid))
			{
				bulletDic.Remove(uid);
			}
		}

    }
}

