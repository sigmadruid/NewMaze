using UnityEngine;
using System.Collections;

public class Bars : MonoBehaviour {
	private float lastCutOffValue = 0f;
	public float cutOffValue = 0;
	private UITexture uiTexture = null;
	private Material barMaterial = null;
	// Use this for initialization
	IEnumerator Start () {
		uiTexture = GetComponent<UITexture>();
		if(uiTexture == null || uiTexture.material == null)
		{
			Debug.LogWarning("Could not find UITexture or material");
			enabled = false;
			yield break;
		}
		//wait two frames so we know drawCall exists
		yield return null; yield return null;

		barMaterial = uiTexture.drawCall.dynamicMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		if(barMaterial == null)
			return;
		
		if(cutOffValue < 0f)
			cutOffValue = 0f;
		else if(cutOffValue > 1f)
			cutOffValue = 1f;
		
		if(lastCutOffValue != cutOffValue)
		{
			lastCutOffValue = cutOffValue;
			barMaterial.SetFloat("_Cutoff", cutOffValue);
		}
	}
}


