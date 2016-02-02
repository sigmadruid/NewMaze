using UnityEngine;
using System;

namespace Base
{
	public enum ObjectType
	{
		GameObject,
		Mesh,
		Texture,
		Materal,
		Config,
		None
	}

	public class ResourceConst
	{
		public static string ASSET_BUNDLE_PATH = Application.streamingAssetsPath + "/Asset Bundles/";
	}

}