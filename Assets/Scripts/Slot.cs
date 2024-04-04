using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform _root;
    private void Start()
    {
        _root = transform.root;
        Debug.Log(_root);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _root.BroadcastMessage("BeginDrag", transform, SendMessageOptions.DontRequireReceiver);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        _root.BroadcastMessage("Drag", transform, SendMessageOptions.DontRequireReceiver);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _root.BroadcastMessage("EndDrag", transform, SendMessageOptions.DontRequireReceiver);
    }
}
