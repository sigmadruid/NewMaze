using UnityEngine;
using UnityEngine.UI;

using System;

namespace Base
{
    public class UITargetTexture : MonoBehaviour
    {
        private Camera targetCamera;

        private RawImage image;
        private RenderTexture rt;
        private GameObject model;

        void Awake()
        {
            image = GetComponent<RawImage>();
            rt = new RenderTexture((int)image.GetWidth(), (int)image.GetHeight(), 16);
            rt.Create();
            image.texture = rt;

            targetCamera = RootTransform.Instance.TargetCamera;
            targetCamera.targetTexture = rt;
        }

        void OnDestroy()
        {
            rt.Release();
            GameObject.Destroy(model);
        }

        void OnEnable()
        {
            targetCamera.enabled = true;
        }

        void OnDisable()
        {
            targetCamera.enabled = false;
        }

        public void Set(GameObject go)
        {
            if(model != null)
            {
                GameObject.Destroy(model);
            }
            model = go;
            float height = model.GetComponentInChildren<SkinnedMeshRenderer>().bounds.extents.y;
            model.transform.SetParent(targetCamera.transform);
            model.transform.localScale = Vector3.one;
            model.transform.localPosition = Vector3.down * height;
            model.transform.localEulerAngles = new Vector3(0, -180, 0);

            targetCamera.orthographicSize = height;
        }

    }
}

