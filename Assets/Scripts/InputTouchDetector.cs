using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTouchDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public float resultX, resultY;
    static public InputTouchDetector instance;
    [Header("Чувствительность касания")][SerializeField]private float sens;
    private void Awake()
    {
        if(InputTouchDetector.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        InputTouchDetector.instance = this;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
            if (eventData.delta.x > 1)
            {
                resultX = eventData.delta.normalized.x * sens;
            }
            if (eventData.delta.x < -1)
            {
                resultX = eventData.delta.normalized.x * sens;
            }

            if (eventData.delta.y > 1)
            {
                resultY = eventData.delta.normalized.y * sens;
            }
            if (eventData.delta.y < -1)
            {
                resultY = eventData.delta.normalized.y * sens;
            }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        resultY = 0;
        resultX = 0;
    }
}
