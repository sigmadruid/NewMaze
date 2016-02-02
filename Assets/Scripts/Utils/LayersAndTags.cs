using System;
namespace GameLogic
{
	public static class Layers
	{
		public const int LayerHero 			= 1 << 10;
		public const int LayerMonster 		= 1 << 11;
		public const int LayerBlock 		= 1 << 12;
		public const int LayerNPC 			= 1 << 13;
		public const int LayerExploration 	= 1 << 14;
		public const int LayerBullet	 	= 1 << 15;
	}
    public static class Tags
    {
        public const string Hero = "TagHero";
        public const string Monster = "TagMonster";
        public const string NPC = "TagNPC";
        public const string TownHero = "TagTownHero";
		public const string Block = "TagBlock";
		public const string Exploration = "TagExploration";
    }

}

