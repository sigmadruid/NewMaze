using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

public enum PositionType
{
	Monster,
	NPC,
	Exploration,
}
public class PositionScript : MonoBehaviour
{
	public PositionType positionType = PositionType.Monster;

	[HideInInspector]
	public bool Available = true;

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		Quaternion rotation = Quaternion.LookRotation(transform.forward);
		Handles.color = GetHandleColor();
		Handles.ArrowCap(0, transform.position, rotation, 1f);
		Handles.SphereCap(0, transform.position, Quaternion.identity, 1f);
	}
	private Color GetHandleColor()
	{
		switch(positionType)
		{
			case PositionType.Monster:		return Color.red;
			case PositionType.NPC:			return Color.green;
			case PositionType.Exploration:	return Color.blue;
		}
		return Color.white;
	}
#endif

}

