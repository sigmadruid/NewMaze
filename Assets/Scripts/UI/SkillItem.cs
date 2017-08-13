using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using Battle;

public class SkillItem : MonoBehaviour
{
    public Image imageIcon;
    public Image imageMask;

    public Skill Skill;

    void Update()
    {
        if(Skill != null && Skill.CD > 0)
        {
            imageMask.fillAmount = Skill.CD / Skill.Data.CD;
        }
    }

    public void SetData(Skill skill)
    {
        this.Skill = skill;
        bool hasSkill = skill != null;
        imageIcon.gameObject.SetActive(hasSkill);
        if(hasSkill)
        {
            imageIcon.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_SKILLS, skill.Data.Res2D);
        }
    }

   
}

