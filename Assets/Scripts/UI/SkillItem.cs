using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using Battle;

public class SkillItem : MonoBehaviour
{
    public Image imageIcon;
    public Image imageMask;

    private Skill skill;

    void Update()
    {
        if(skill != null && skill.CD > 0)
        {
            imageMask.fillAmount = skill.CD / skill.Data.CD;
        }
    }

    public void SetData(Skill skill)
    {
        this.skill = skill;
        imageIcon.gameObject.SetActive(skill != null);
        if(skill != null)
        {
            imageIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_SKILLS, skill.Data.Res2D);
        }
    }

   
}

