using UnityEngine;
using System.Collections;

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
        public float vibrationScope = 0.1f;

    	public Transform playerTransofrm;

        [HideInInspector]
        public Camera Camera;

        private RaycastHit[] prevHitInfos;

        private DOTweenAnimation tween;
        private bool isVibrating;

    	public static Camera3DScript Instance { get; private set; }

    	void Awake()
    	{
    		Instance = this;
            Camera = GetComponent<Camera>();
            tween = GetComponent<DOTweenAnimation>();
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
            if (playerTransofrm != null && !isVibrating)
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

        public void Vibrate()
        {
            isVibrating = true;
            transform.DOShakePosition(0.15f, Vector3.up * 0.15f).OnComplete(() =>
                {
                    isVibrating = false;
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