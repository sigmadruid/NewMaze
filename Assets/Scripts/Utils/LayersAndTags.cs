using UnityEngine;
using System;
namespace Base
{
	public static class Layers
	{
        public static readonly int LayerRenderTexture = LayerMask.GetMask("LayerRenderTexture");
        public static readonly int LayerHero = LayerMask.GetMask("LayerHero");
        public static readonly int LayerMonster = LayerMask.GetMask("LayerMonster");
        public static readonly int LayerNPC = LayerMask.GetMask("LayerNPC");
        public static readonly int LayerBlock = LayerMask.GetMask("LayerBlock");
        public static readonly int LayerExploration = LayerMask.GetMask("LayerExploration");
        public static readonly int LayerBullet = LayerMask.GetMask("LayerBullet");
        public static readonly int LayerMockBlock = LayerMask.GetMask("LayerMockBlock");
        public static readonly int LayerItem = LayerMask.GetMask("LayerItem");
        public static readonly int LayerWalkSurface = LayerMask.GetMask("LayerWalkSurface");
	}
    public static class Tags
    {
        public const string Hero = "TagHero";
        public const string Monster = "TagMonster";
        public const string NPC = "TagNPC";
        public const string TownHero = "TagTownHero";
		public const string Block = "TagBlock";
        public const string Exploration = "TagExploration";
		public const string Trap = "TagTrap";
    }

}

