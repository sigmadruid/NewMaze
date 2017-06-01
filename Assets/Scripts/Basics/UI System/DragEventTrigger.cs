using UnityEngine;
using UnityEngine.EventSystems;

using System;

namespace Base
{
    public class DragEventTrigger : EventTrigger
    {
        public delegate void VectorDelegate (GameObject go, Vector2 delta);

        public VectorDelegate onDrag;

        static public DragEventTrigger Get (GameObject go)
        {
            DragEventTrigger listener = go.GetComponent<DragEventTrigger>();
            if (listener == null) listener = go.AddComponent<DragEventTrigger>();
            return listener;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if(onDrag != null) onDrag(gameObject, eventData.delta);
        }
    }
}

