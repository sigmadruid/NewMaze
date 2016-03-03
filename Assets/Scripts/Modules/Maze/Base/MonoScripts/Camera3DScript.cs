using UnityEngine;
using System.Collections;

using Base;
using StaticData;
using GameLogic;

namespace GameLogic
{
    public class Camera3DScript : MonoBehaviour 
    {
    	public float height = 10f;
    	public float distance = 6f;

    	public Transform playerTransofrm;

        [HideInInspector]
        public Camera Camera;

    	private Transform cachedTransform;
    	private GameObject prevObstacle;

    	public static Camera3DScript Instance { get; private set; }

    	void Awake()
    	{
    		Instance = this;
            Camera = GetComponent<Camera>();
    		cachedTransform = transform;
    	}
    	void Start () 
    	{
    		StartCoroutine(SlowUpdate());
    	}
    	void OnDestroy()
    	{
            Instance = null;
            Camera = null;
    		StopAllCoroutines();
    	}
    	void LateUpdate () 
    	{
    		if (playerTransofrm != null)
    		{
    			cachedTransform.position = playerTransofrm.position + Vector3.forward * (-distance) + Vector3.up * height + Vector3.right * distance;
    			cachedTransform.LookAt(playerTransofrm.position);
    		}
    	}
    	private IEnumerator SlowUpdate()
    	{
    		while (true)
    		{
    			CheckObstacleObjects();

    			yield return GlobalConfig.CameraConfig.CheckObstacleDelay;
    		}
    	}

    	#region Check Obstacle

    	private void CheckObstacleObjects()
    	{
    		if (prevObstacle != null)
    		{
    			SetTransparent(prevObstacle, false);
    		}

    		if (playerTransofrm != null)
    		{
    			float distance = Vector3.Distance(cachedTransform.position, playerTransofrm.position);
    			Vector3 direction = playerTransofrm.position - cachedTransform.position;

    			RaycastHit hitInfo;
    			if (Physics.Raycast(cachedTransform.position, direction, out hitInfo, distance - 1f, Layers.LayerBlock))
    			{
    				GameObject go = hitInfo.collider.gameObject;
    				SetTransparent(go, true);
    				prevObstacle = go;
    			}
    		}
    	}
    	private void SetTransparent(GameObject go, bool transparent)
    	{
    		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
    		for (int i = 0; i < renderers.Length; ++i)
    		{
    			Material material = renderers[i].material;
    			if (transparent && material.shader == Utils.DiffuseShader)
    			{
    				material.shader = Utils.TransparentShader;
    				material.SetColor("_Color", new Color(1, 1, 1, 0.4f));
    			}
    			else if (material.shader == Utils.TransparentShader)
    			{
    				material.shader = Utils.DiffuseShader;
    			}
    		}
    	}

    	#endregion

    }
}