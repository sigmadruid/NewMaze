using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;

using Base;
using StaticData;

public class SoulAttachItem : MonoBehaviour
{
    public Image imageIcon;

    public Text textName;

    public Text textAttr;

    public Image imageHover;
    public Image imageSelected;

    public void SetData(ItemData data)
    {
        textName.text = TextDataManager.Instance.GetData(data.Name);
        imageIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_ITEM, data.Res2D);

        int buffKid = int.Parse(data.Param1);
        BuffData buffData = BuffDataManager.Instance.GetData(buffKid);
        StringBuilder sb = new StringBuilder();

        var enumRatio = buffData.AttributeRatioDic.GetEnumerator();
        while(enumRatio.MoveNext())
        {
            BattleAttribute attr = (BattleAttribute)enumRatio.Current.Key;
            string attrStr = TextDataManager.Instance.GetData("common." + attr.ToString());
            float val = enumRatio.Current.Value;
            string buffStr = string.Format("{0} + {1}%\n", attrStr, val * 100);
            sb.Append(buffStr);
        }

        var enumRaise = buffData.AttributeRaiseDic.GetEnumerator();
        while(enumRatio.MoveNext())
        {
            BattleAttribute attr = (BattleAttribute)enumRaise.Current.Key;
            string attrStr = TextDataManager.Instance.GetData("common." + attr.ToString());
            int val = enumRaise.Current.Value;
            string buffStr = string.Format("{0} + {1}\n", attrStr, val);
            sb.Append(buffStr);
        }
        textAttr.text = sb.ToString();
    }

    private bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            isSelected = value;
            Utils.SetActive(imageSelected.gameObject, value);
        }
    }

}

