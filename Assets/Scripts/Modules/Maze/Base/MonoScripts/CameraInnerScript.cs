using UnityEngine;
using System.Collections;

using Base;
using StaticData;
using GameLogic;

namespace GameLogic
{
    public class CameraInnerScript : MonoBehaviour
    {
        public static CameraInnerScript Instance { get; private set; }

        public Camera Camera;

        void Awake()
        {
            Instance = this;
            Camera = GetComponent<Camera>();
        }
        void OnDestroy()
        {
            Instance = null;
        }
    }
}

