using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;
using DG.Tweening;

namespace GameLogic
{
    public class Camera3DScript : MonoBehaviour 
    {
    	public float height = 10f;
    	public float distance = 6f;
        public float vibrationScope = 0.15f;
        public float vibrationDuration = 0.15f;
        public float zoomDistance = 3f;
        public float zoomDuration = 0.3f;

    	public Transform playerTransofrm;

        private Dictionary<int, CameraVibration> vibrationDic = new Dictionary<int, CameraVibration>()
        {
            {0, null},
            {1, new CameraVibration(0.15f, 0.15f)},
            {2, new CameraVibration(0.2f, 0.2f)},
            {3, new CameraVibration(0.3f, 0.3f)},
        };

        [HideInInspector]
        public Camera Camera;

        private RaycastHit[] prevHitInfos;

        private bool isFollowing = true;

    	public static Camera3DScript Instance { get; private set; }

    	void Awake()
    	{
    		Instance = this;
            Camera = GetComponent<Camera>();
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
            if (playerTransofrm != null && isFollowing)
    		{
                transform.position = playerTransofrm.position + Vector3.forward * (-distance) + Vector3.up * height + Vector3.right * distance;
                transform.LookAt(playerTransofrm.position);
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

        public void Vibrate(CameraVibration.Type type)
        {
            isFollowing = false;
            var vibration = vibrationDic[(int)type];
            if(vibration != null)
            {
                transform.DOShakePosition(vibration.duration, Vector3.up * vibration.scope).OnComplete(() =>
                    {
                        isFollowing = true;
                    });
            }
        }

        public void Zoom(bool zoomIn)
        {
            isFollowing = false;
            Vector3 forward = zoomIn ? Adam.Instance.WorldPosition - transform.position : transform.position - Adam.Instance.WorldPosition;
            forward = forward.normalized;
            transform.DOMove(transform.position + forward * zoomDistance, zoomDuration).OnComplete(() =>
                {
                    if (!zoomIn)
                        isFollowing = true;
                });
        }

    	#region Check Obstacle

    	private void CheckObstacleObjects()
    	{
            if (prevHitInfos != null && prevHitInfos.Length > 0)
    		{
                foreach(RaycastHit hitInfo in prevHitInfos)
                {
                    SetTransparent(hitInfo.collider.gameObject, false);
                }
    		}

    		if (playerTransofrm != null)
    		{
                float distance = Vector3.Distance(transform.position, playerTransofrm.position);
                Vector3 direction = playerTransofrm.position - transform.position;

                RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, direction, distance - 1f, Layers.LayerBlock);
                if (hitInfos != null && hitInfos.Length > 0)
    			{
                    foreach(RaycastHit hitInfo in hitInfos)
                    {
                        SetTransparent(hitInfo.collider.gameObject, true);
                    }
                    prevHitInfos = hitInfos;
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