using System.Collections;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 startPos;
    private Vector3 hideDownTargetPos;
    private float speed = 600f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition;  
        hideDownTargetPos = startPos + Vector3.down * 450f;
        rectTransform.localPosition = hideDownTargetPos;  
    }

    public void DownShow()
    {
        if(rectTransform.localPosition == hideDownTargetPos)
        StartCoroutine(ShowCoroutine());
        GameEventSystem.SetInputsActive(false);
    }

    private IEnumerator ShowCoroutine()
    {
        while (Vector3.Distance(rectTransform.localPosition, startPos) > 1f) 
        {
            yield return null;
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, startPos, speed * Time.deltaTime);
        }
        rectTransform.localPosition = startPos;
    }

    private IEnumerator HideDownCoroutine()
    {
        while (Vector3.Distance(rectTransform.localPosition, hideDownTargetPos) > 1f)
        {
            yield return null;
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, hideDownTargetPos, speed * Time.deltaTime);
        }
        rectTransform.localPosition = hideDownTargetPos;
        GameEventSystem.SetInputsActive(true);
    }
    public void DownHide()
    {
        if (rectTransform.localPosition == startPos) 
            StartCoroutine(HideDownCoroutine());
    }
}
