using UnityEngine;

using System;

using Base;
using Battle;
using StaticData;

namespace GameLogic
{
	public class Bullet : Entity
	{
		private const float LIFE_TIME = 3f;
		
		public new BulletData Data
		{
			get { return data as BulletData; }
			protected set { data = value; }
		}

		public new BulletScript Script
		{
			get { return script as BulletScript; }
			protected set { script = value; }
		}

		public AttackContext AttackContext;

		private BattleProxy battleProxy;

        private bool isHit;
		private float timeCounter;

        public void Start(Vector3 emitPosition, Vector3 targetPosition)
		{
			Script.transform.position = emitPosition;
            Script.transform.localRotation = Quaternion.LookRotation(targetPosition - emitPosition);
            Script.SetState(BulletState.Normal);
            isHit = false;
            timeCounter = 0f;
		}

		protected override void Update()
		{
            if(isHit) return;

			Vector3 velocity = Vector3.forward * Data.Speed;
			Script.transform.Translate(velocity * Time.deltaTime, Space.Self);

			timeCounter += Time.deltaTime;
			if (timeCounter > LIFE_TIME)
			{
                OnDestroy();
			}
		}

        private void OnHit(Collider other)
		{
            if(isHit) return;

            if(AttackContext.CasterSide == Side.Hero)
            {
                if (!other.CompareTag(Tags.Hero))
                {
                    isHit = true;
                    if (other.CompareTag(Tags.Monster))
                    {
                        string uid = other.GetComponent<MonsterScript>().Uid;
                        Monster monster = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().GetMonster(uid);
                        battleProxy.DoAttackMonster(monster, AttackContext);
                    }
                    Script.SetState(BulletState.After, Data.EndDuration);
                }
            }
            else if(AttackContext.CasterSide == Side.Monster)
            {
                if (!other.CompareTag(Tags.Monster))
                {
                    isHit = true;
                    if (other.CompareTag(Tags.Hero))
                    {
                        battleProxy.DoAttackHero(AttackContext);
                    }
                    Script.SetState(BulletState.After, Data.EndDuration);
                }
            }
			
		}
		private void OnDestroy()
		{
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BULLET_DESPAWN, this);
			Bullet.Recycle(this);
		}

		public static Bullet Create(int kid)
		{
			Bullet bullet = new Bullet();
			bullet.Uid = Guid.NewGuid().ToString();
			bullet.Data = BulletDataManager.Instance.GetData(kid) as BulletData;
			bullet.Script = ResourceManager.Instance.LoadAsset<BulletScript>(ObjectType.GameObject, bullet.Data.GetResPath());
			bullet.Script.transform.parent = RootTransform.Instance.BulletRoot;	
			bullet.Script.CallbackUpdate = bullet.Update;
			bullet.Script.CallbackHit = bullet.OnHit;
			bullet.Script.CallbackDestroy = bullet.OnDestroy;
			bullet.battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
			
			return bullet;
		}
		public static void Recycle(Bullet bullet)
		{
			if (bullet != null)
			{
				bullet.Data = null;
				bullet.Script.CallbackUpdate = null;
				bullet.Script.CallbackHit = null;
				bullet.Script.CallbackDestroy = null;
				bullet.Script.StopAllCoroutines();
				ResourceManager.Instance.RecycleAsset(bullet.Script.gameObject);
				bullet.Script = null;
				bullet.battleProxy = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null bullet!");
			}
		}
	}
}

