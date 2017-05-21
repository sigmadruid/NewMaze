using UnityEngine;
using System;

namespace Base
{
    public static class PanelUtils
    {
        public const string ATLAS_COMMON = "Atlas/Common/";
        public const string ATLAS_NEWCOMMON = "Atlas/NewCommon/";
        public const string ATLAS_PORTRAIT = "Atlas/Portrait/";
        public const string ATLAS_ITEM = "Atlas/Item/";

        public static void ClearChildren(Transform root)
        {
            while (root.childCount > 0)
            {
                Transform child = root.GetChild(0);
                child.parent = null;
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Sprite CreateSprite(string atlas, string name)
        {
            Texture2D texture = Resources.Load<Texture2D>(atlas + name);
            if(texture == null)
            {
                BaseLogger.LogFormat("Can't load Texture2d: {0}-{1}", atlas, name);
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}

