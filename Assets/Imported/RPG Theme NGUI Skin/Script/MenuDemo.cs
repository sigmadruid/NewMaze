/*
 *  MenuDemo.cs
 *   This script is for demonstration purposes only!
*/

using UnityEngine;
using System.Collections;

public class MenuDemo : MonoBehaviour {
    //windows
    GameObject inventoryWindow, questWindow, skillWindow, settingsWindow;
    //tooltips
    GameObject skillTooltip, itemTooltip, genericTooltip;

    static string[] ToolTipableNames = { "ItemHP", "SkillItem", "SkillItemDisabled", "Stats" };
	// Use this for initialization
	void Start () {
        //windows
        inventoryWindow = GameObject.Find("InventoryWindow");    
        questWindow = GameObject.Find("QuestWindow");        
        skillWindow = GameObject.Find("SkillWindow");
        settingsWindow = GameObject.Find("SettingsWindow");   

        //tooltips
        skillTooltip = GameObject.Find("Tooltip Skill");
        skillTooltip.SetActive(false);

        itemTooltip = GameObject.Find("Tooltip Item");
        itemTooltip.SetActive(false);

        genericTooltip = GameObject.Find("Tooltip Generic");
        genericTooltip.SetActive(false);


        //add onClick handlers on the buttons
        foreach (UIButton button in transform.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(button.gameObject).onClick += OnButtonClick;
        }

        //add onHover handlers on particular elements
        foreach (string toolTipableName in ToolTipableNames)
        {
            foreach (Transform t in FindObjectsOfType(typeof(Transform)))
            {
                if (t.name.Contains(toolTipableName))
                    UIEventListener.Get(t.gameObject).onHover += OnHover;
            }
        }

        //Hide windows
        inventoryWindow.SetActive(false);
        questWindow.SetActive(false);
        skillWindow.SetActive(false);
        settingsWindow.SetActive(false);
	}

    void OnHover(GameObject target, bool isOver)
    {
        switch (target.name)
        {
            case "SkillItemDisabled":
            case "SkillItem":
                skillTooltip.SetActive(isOver);
                SetTooltipPos(target.transform.position, skillTooltip);
                break;

            case "ItemHP":
                itemTooltip.SetActive(isOver);
                SetTooltipPos(target.transform.position, itemTooltip);
                break;
        
            case "Stats":            
                genericTooltip.SetActive(isOver);
                SetTooltipPos(target.transform.position, genericTooltip);
                break;
        }
    }

    void SetTooltipPos(Vector3 targetPos, GameObject tooltipWindow)
    {
        Vector3 newPos = targetPos;
        Vector3 extends = tooltipWindow.GetComponent<Collider>().bounds.extents;
        if(targetPos.x > 0)
            newPos.x -= (extends.x + 0.1f); //offset of 0.1f
        else
            newPos.x += (extends.x + 0.1f); //offset of 0.1f
        newPos.z = -0.1f; //always force the tooltip window to the front
        tooltipWindow.transform.position = newPos;
    }

    void OnButtonClick(GameObject target)
    {
       
        switch (target.name)
        {
            case "BtnInventory":
                inventoryWindow.SetActive(!inventoryWindow.activeInHierarchy);
                break;

            case "BtnQuest":
                questWindow.SetActive(!questWindow.activeInHierarchy);
                break;

            case "BtnSkill":
                skillWindow.SetActive(!skillWindow.activeInHierarchy);
                break;

            case "BtnSettings":
                settingsWindow.SetActive(!settingsWindow.activeInHierarchy);
                break;

            case "BtnCloseInventory":
                inventoryWindow.SetActive(false);
                break;

            case "BtnCloseQuest":
                questWindow.SetActive(false);
                break;

            case "BtnDecline":
                questWindow.SetActive(false);
                break;

            case "BtnAccept":
                questWindow.SetActive(false);
                break;

            case "BtnCloseSkill":
                skillWindow.SetActive(false);
                break;

            case "BtnCloseSettings":
                settingsWindow.SetActive(false);
                break;

            case "BtnQuit":
                settingsWindow.SetActive(false);
                break;

            default:
                break;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
