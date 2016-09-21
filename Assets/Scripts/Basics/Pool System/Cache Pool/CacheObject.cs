using UnityEngine;

namespace Base
{
	public class CacheObject
	{
		public delegate void DestroyDelegate();

		public int id;

		public string objName;

		public UnityEngine.Object obj;

		public ObjectType type;

		public float remainingLife;

		public int referCount;

		public DestroyDelegate onDestroy;

		public CacheObject(float life)
		{
			this.life = life;
		}

		private float life;
		public float Life 
		{
			get
			{
				return life;
			}
		}
	}

}