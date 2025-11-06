using System.Collections;
using UnityEngine;

public class MainMenuPopup : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 targetPos;
    private Vector3 hideLeftTargetPos;
    private Vector3 hideRightTargetPos;
    private float speed = 20f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPos = rectTransform.position;
        hideLeftTargetPos = targetPos + Vector3.left * 15f;
        hideRightTargetPos = targetPos + Vector3.right * 15f;
    }

    void OnEnable()
    {
        rectTransform.position = targetPos + Vector3.right * 15f;
        StartCoroutine(ShowRightCoroutine());
    }

    private IEnumerator ShowRightCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, targetPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, targetPos, speed * Time.deltaTime);
        }
        rectTransform.position = targetPos;
    }

    private IEnumerator HideLeftCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, hideLeftTargetPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, hideLeftTargetPos, speed * Time.deltaTime);
        }
        rectTransform.position = hideLeftTargetPos;
        gameObject.SetActive(false);
    }
    private IEnumerator HideRightCoroutine()
    {
        while (Vector3.Distance(rectTransform.position, hideRightTargetPos) > 0.1f)
        {
            yield return null;
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, hideRightTargetPos, speed * Time.deltaTime);
        }
        rectTransform.position = hideRightTargetPos;
        gameObject.SetActive(false);
    }
    public void LeftHide()
    {
        rectTransform.position = targetPos;
        StartCoroutine (HideLeftCoroutine());
    }
}
