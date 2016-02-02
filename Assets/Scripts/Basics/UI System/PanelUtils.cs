using UnityEngine;
using System;

namespace Base
{
    public static class PanelUtils
    {
        public static void ClearChildren(Transform root)
        {
            while (root.childCount > 0)
            {
                Transform child = root.GetChild(0);
                child.parent = null;
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}

