using UnityEngine;
using System;
using System.IO;

using Base;

namespace StaticData
{
	public enum BlockType
	{
		Passage = 1,
		Room,
		Hall,
	}
	public enum PassageType
	{
		One = 1,
		TwoLine,
		TwoTurn,
		Three,
		Four
	}

	public class BlockData : EntityData
	{
		public BlockType BlockType;
		public PassageType PassageType;

		public int Cols = 1;
		public int Rows = 1;
		public int LeftOffset;

		public string Res3D;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				if (BlockType == BlockType.Passage)
				{
					resPath = Utils.CombinePath("MazeBlocks", PassageType, Res3D);
				}
				else
				{
					resPath = Utils.CombinePath("MazeBlocks", BlockType, Res3D);
				}
			}
			return resPath;
		}
	}
}

