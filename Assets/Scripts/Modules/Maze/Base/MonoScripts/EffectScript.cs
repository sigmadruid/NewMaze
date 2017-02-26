using UnityEngine;

using System;

using Base;
using DG.Tweening;

namespace GameLogic
{
    public class EffectScript : MonoBehaviour
    {
        protected Material material;

        public void SetTransparent(bool isTransparent, float alpha = 0.2f)
        {
            InitMaterial();
            if(isTransparent)
            {
                material.shader = Utils.TransparentShader;
                material.SetColor("_Color", new Color(1, 1, 1, alpha));
            }
            else
            {
                material.shader = Utils.DiffuseShader;
            }
        }

        public void SetEmissionColor(Color color, float duration)
        {
            InitMaterial();
            material.EnableKeyword("_EMISSION");
            material.DOColor(color, "_EmissionColor", duration);
        }

        private void InitMaterial()
        {
            if(material == null)
            {
                material = GetComponentInChildren<Renderer>().material;
            }
        }
    }
}

