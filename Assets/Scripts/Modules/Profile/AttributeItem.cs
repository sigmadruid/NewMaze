using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Base;
using StaticData;

public class AttributeItem : MonoBehaviour 
{
    public Image imageIcon;
    public Text textName;
    public Text textValue;

    public void SetData(BattleAttribute attribute, float value)
    {
        imageIcon.sprite = GetIcon(attribute);
        textName.text = attribute.ToString();
        textValue.text = Mathf.RoundToInt(value).ToString();
    }

    private Sprite GetIcon(BattleAttribute attribute)
    {
        string spriteName = null;
        switch(attribute)
        {
            case BattleAttribute.HP:
                spriteName = "hp_icon";
                break;
            case BattleAttribute.Attack:
                spriteName = "attack_icon";
                break;
            case BattleAttribute.Defense:
                spriteName = "defense_icon";
                break;
            case BattleAttribute.Critical:
                spriteName = "critical_icon";
                break;
            case BattleAttribute.Dodge:
                spriteName = "dodge_icon";
                break;
            case BattleAttribute.AttackSpeed:
                spriteName = "attackspeed_icon";
                break;
            case BattleAttribute.MoveSpeed:
                spriteName = "movespeed_icon";
                break;
        }
        return PanelUtils.CreateSprite(PanelUtils.ATLAS_NEWCOMMON, spriteName);
    }
}
