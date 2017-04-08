using UnityEngine;

using System;

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

    public Transform PoolRoot;

	void Awake()
	{
		Instance = this;
	}
}

