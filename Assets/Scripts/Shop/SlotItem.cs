using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotItem : MonoBehaviour
{
    Slot _slot;
    private float resetPositionY = -136f;
    private float startPositionY = 34;
    RectTransform rectTransform;
    private void Start()
    {
        _slot = GetComponentInParent<Slot>();
        rectTransform = GetComponent<RectTransform>();
        startPositionY = 102;
        resetPositionY = -34;
    }

    void Update()
    {
        
        rectTransform.localPosition += Vector3.down * _slot.Speed * Time.deltaTime;

        if (rectTransform.localPosition.y < resetPositionY)
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, startPositionY-Mathf.Abs(resetPositionY-rectTransform.localPosition.y), 0);
    }
}
