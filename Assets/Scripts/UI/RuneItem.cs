using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using StaticData;
using GameLogic;

public class RuneItem : MonoBehaviour
{
    public Image imageIcon;
    public Image imageMask;
    public Text textCount;

    public ItemInfo Info;
    public RuneType Type;

    void Update()
    {
        if(Info != null && Info.UseInterval > 0)
        {
            imageMask.fillAmount = Info.UseInterval / Info.Data.UseInterval;
            Info.UseInterval -= Time.deltaTime;
        }
    }

    public void SetData(ItemInfo info)
    {
        Info = info;

        bool hasInfo = info != null;
        Utils.SetActive(imageIcon.gameObject, hasInfo);
        Utils.SetActive(imageMask.gameObject, hasInfo);
        Utils.SetActive(textCount.gameObject, hasInfo);
        if(hasInfo)
        {
            imageIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_ITEM, info.Data.Res2D);
            textCount.text = info.Count.ToString();
            Type = RuneProxy.GetRuneType(info.Data);
        }
    }

    public void Use()
    {
        Info.UseInterval = Info.Data.UseInterval;
    }
}

