using System.Collections;
using UnityEngine;

public class MainMenuPopup : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 startPos;
    private Vector3 hideLeftTargetPos;
    private Vector3 hideRightTargetPos;
    private float speed = 30f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.position;
        hideLeftTargetPos = startPos + Vector3.left * 15f;
        hideRightTargetPos = startPos + Vector3.right * 15f;
    }
    public void HideAtStart()
    {
        rectTransform.position = hideRightTargetPos;
    }

    public void RightShow()
    {
        rectTransform.position = hideRightTargetPos;
        StartCoroutine(ShowCoroutine());
    }
    public void LeftShow()
    {
        rectTransform.position = hideLeftTargetPos;
        StartCoroutine(ShowCoroutine());
    }
    public void LeftHide()
    {
        Debug.Log("Zestalo sie 1");
        rectTransform.position = startPos;
        Debug.Log("Zestalo sie 2");
        StartCoroutine(HideLeftCoroutine());
    }
    public void RightHide()
    {
        rectTransform.position = startPos;
        StartCoroutine(HideRightCoroutine());
    }


    private IEnumerator ShowCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, startPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, startPos, speed * Time.deltaTime);
        }
        rectTransform.position = startPos;
    }

    private IEnumerator HideLeftCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, hideLeftTargetPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, hideLeftTargetPos, speed * Time.deltaTime);
        }
        rectTransform.position = hideLeftTargetPos;
    }
    private IEnumerator HideRightCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, hideRightTargetPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, hideRightTargetPos, speed * Time.deltaTime);
        }
        rectTransform.position = hideRightTargetPos;
    }
}
