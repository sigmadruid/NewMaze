using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

using DG.Tweening;

namespace GameUI
{
    public class BubbleItem : BaseScreenItem
    {
        public int BaseWidth;

        public int MaxCharPerLine;
        public int BaseHeight;
        public int LineHeight;

        public Image imgBackground;
        public Text txtName;
        public Text txtContent;

        public void Show(bool state, string name = null, string content = null)
        {
            Vector2 size = CalculateSize(content);
            if(state)
            {
                txtName.text = name;
                txtContent.text = content;

                gameObject.SetActive(true);
                rectTransform.sizeDelta = new Vector2(0, size.y);
                rectTransform.DOSizeDelta(size, 0.5f).OnComplete(() =>
                    {
                        gameObject.SetActive(true);
                    });
            }
            else
            {
                rectTransform.DOSizeDelta(new Vector2(0, size.y), 0.5f).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
            }
        }

        private Vector2 CalculateSize(string content)
        {
            int length = content != null ? content.Length : 0;
            float height = BaseHeight + LineHeight * length / MaxCharPerLine;
            return new Vector2(BaseWidth, height);
        }

        public static BubbleItem Create(Vector3 worldPosition, string name, string content)
        {
            BubbleItem item = PopupManager.Instance.CreateItem<BubbleItem>(RootTransform.Instance.UIIconRoot);
            item.UpdatePosition(worldPosition + Vector3.up * 2.5f);
            item.Show(true, name, content);
            return item;
        }
        public static void Recycle(BubbleItem item)
        {
            if (item != null)
            {
                item.Show(false);
                PopupManager.Instance.RemoveItem(item.gameObject);
            }
            else
            {
                BaseLogger.Log("Recyle a null BubbleItem!");
            }
        }
    }
}

