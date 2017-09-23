using UnityEngine;

using System;

using GameLogic;

public class RootTransform : MonoBehaviour
{
	public static RootTransform Instance { get; private set;}

	public Transform UIIconRoot;
	public Transform UIPanelRoot;

	public Transform BlockRoot;
	public Transform MockBlockRoot;
    public Transform WalkSurfaceRoot;

	public Transform MonsterRoot;
	public Transform NPCRoot;
	public Transform ExplorationRoot;
	public Transform BulletRoot;
    public Transform DropRoot;

    public Camera TargetCamera;

    public Transform PoolRoot;

	void Awake()
	{
		Instance = this;
        if (UIPanelRoot != null)
            UIPanelRoot.parent.GetComponent<Canvas>().worldCamera = CameraUIScript.Instance.Camera;
	}
    void OnDestroy()
    {
        Instance = null;
    }
}

