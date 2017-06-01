using UnityEngine;
using UnityEngine.EventSystems;

using System;

namespace Base
{
    public class ClickEventTrigger : MonoBehaviour, IPointerClickHandler
    {
        public delegate void VoidDelegate (GameObject go);

        public VoidDelegate onClick;

        static public ClickEventTrigger Get (GameObject go)
        {
            ClickEventTrigger listener = go.GetComponent<ClickEventTrigger>();
            if (listener == null) listener = go.AddComponent<ClickEventTrigger>();
            return listener;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(onClick != null)     onClick(gameObject);
        }
    }
}

