using System;

using Base;
using Battle;

namespace GameLogic
{
	public class ApplicationFacade : Facade
	{
		public new static ApplicationFacade Instance
		{
			get
			{
				if (m_instance == null)
					m_instance = new ApplicationFacade();
				return m_instance as ApplicationFacade;
			}
		}

		public void Startup()
		{
			RegisterProxy(new MazeProxy());
			RegisterProxy(new BlockProxy());
			RegisterProxy(new HallProxy());
			RegisterProxy(new HeroProxy());
			RegisterProxy(new MonsterProxy());
			RegisterProxy(new BulletProxy());
			RegisterProxy(new BattleProxy());
			RegisterProxy(new NPCProxy());
			RegisterProxy(new ExplorationProxy());
			RegisterProxy(new DropProxy());
			RegisterProxy(new EnvironmentProxy());
            RegisterProxy(new PackProxy());

			RegisterMediator(new BlockMediator());
			RegisterMediator(new MazeMapMediator());
			RegisterMediator(new HeroMediator());
			RegisterMediator(new HallMediator());
			RegisterMediator(new MonsterMediator());
			RegisterMediator(new BulletMediator());
			RegisterMediator(new NPCMediator());
			RegisterMediator(new ExplorationMediator());
			RegisterMediator(new BattleUIMediator());
			RegisterMediator(new DropMediator());
            RegisterMediator(new EnvironmentMediator());
            RegisterMediator(new RecordMediator());
            RegisterMediator(new PackMediator());
            RegisterMediator(new PathfindingMediator());
		}
	}
}

