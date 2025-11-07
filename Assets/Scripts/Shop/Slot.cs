using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float Speed { get; set; }
    void Start()
    {
        Speed = Random.Range(400f, 500f);
        StartCoroutine(StopSlot());
    }
    public IEnumerator MyTestCoroutine()
    {
        yield return new WaitForSeconds(3);
        float newSpeed = Random.Range(400f, 500f);
        while(Speed < newSpeed)
        {
            yield return null;
            Speed += newSpeed / 10;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(StopSlot());
    }

    public IEnumerator StopSlot()
    {
        float stopSpeed = Speed / 100;
        yield return new WaitForSeconds(5);
        {
            while(Speed > 1)
            {
                yield return null;
                Speed-=stopSpeed;
            }
            Speed = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ChooseItem());
    }
    public IEnumerator ChooseItem()
    {
        RectTransform[] slotItems = GetComponentsInChildren<RectTransform>();
        RectTransform closestItem = null;
        foreach(RectTransform item in slotItems){
            if (item == GetComponent<RectTransform>()) 
                continue;
            if(closestItem == null) 
                closestItem = item;
            if (Mathf.Abs(closestItem.localPosition.y) > Mathf.Abs(item.localPosition.y))
                closestItem = item;
        }
        while (Mathf.Abs(closestItem.localPosition.y)>0.5f)
        {
            yield return null;
            Vector3 oldPos = closestItem.localPosition;
            closestItem.localPosition = Vector3.MoveTowards(
                closestItem.localPosition,
                Vector3.zero,
                20 * Time.deltaTime);
            Vector3 difference = closestItem.localPosition - oldPos;
            MoveAllChildrenExceptOne(difference, closestItem);

        }

        StartCoroutine(MyTestCoroutine());
    }

    public void MoveAllChildrenExceptOne(Vector3 offset, RectTransform exception)
    {
        foreach (RectTransform child in GetComponentsInChildren<RectTransform>())
        {
            if (child == transform || child == exception) continue;
            child.localPosition += offset;
        }
    }
}
