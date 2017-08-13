using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

public class SoulAttachItem : MonoBehaviour
{
    public Image imageIcon;

    public Text textName;

    public Text textAttr;

    public Image imageHover;
    public Image imageSelected;

    public void SetData()
    {
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

