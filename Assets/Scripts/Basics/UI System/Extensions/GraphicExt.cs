using UnityEngine;
using UnityEngine.UI;

using System;

namespace Base
{
    public static class GraphicExt
    {
        public static float GetWidth(this Graphic graphic)
        {
            return graphic.rectTransform.sizeDelta.x;
        }
        public static float GetHeight(this Graphic graphic)
        {
            return graphic.rectTransform.sizeDelta.y;
        }
        public static void SetWidth(this Graphic graphic, float width)
        {
            graphic.rectTransform.sizeDelta = new Vector2(width, graphic.rectTransform.sizeDelta.y);
        }
        public static void SetHeight(this Graphic graphic, float height)
        {
            graphic.rectTransform.sizeDelta = new Vector2(graphic.rectTransform.sizeDelta.x, height);
        }
    }
}

