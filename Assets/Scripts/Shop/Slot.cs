using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private float _idleSpeed;
    public float Speed { get; set; }
    [SerializeField] RectTransform _healthPotion;
    [SerializeField] RectTransform _strengthPotion;
    [SerializeField] RectTransform _speedPotion;
    [SerializeField] RectTransform _sword;
    Inventory _inventory;
    RectTransform _closestItem;
    public bool CanBeCollected { get; set; } = false;
    void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        _idleSpeed = 60;
        Speed = _idleSpeed;
    }

    public void StartSpin()
    {
        StartCoroutine(StartSpinCoroutine());
    }
    public void StopSpin()
    {
        StartCoroutine(StopSpinningCoroutine());
    }
    private IEnumerator StartSpinCoroutine()
    {
        float newSpeed = Random.Range(1000, 1200f);
        float i = 0.08f;
        while(Speed < newSpeed)
        {
            i+=0.08f;
            yield return null;
            Speed += newSpeed / 5*i*Time.deltaTime;
        }
    }

    private IEnumerator StopSpinningCoroutine()
    {
        float stopSpeed = Speed / 100;
        {
            while(Speed > 1)
            {
                yield return null;
                Speed-=stopSpeed*Time.deltaTime*100;
            }
            Speed = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ChooseItem());
    }
    private IEnumerator ChooseItem()
    {
        RectTransform[] slotItems = GetComponentsInChildren<RectTransform>();
        _closestItem = null;
        foreach(RectTransform item in slotItems){
            if (item == GetComponent<RectTransform>()) 
                continue;
            if(_closestItem == null)
                _closestItem = item;
            if (Mathf.Abs(_closestItem.localPosition.y) > Mathf.Abs(item.localPosition.y))
                _closestItem = item;
        }
        while (Vector3.Distance(_closestItem.localPosition, Vector3.zero) > 0.5f)
        {
            yield return null;
            Vector3 oldPos = _closestItem.localPosition;
            _closestItem.localPosition = Vector3.MoveTowards(
                _closestItem.localPosition,
                Vector3.zero,
                Vector3.Distance(_closestItem.localPosition, Vector3.zero) *5 * Time.deltaTime);
            Vector3 difference = _closestItem.localPosition - oldPos;
            MoveAllChildrenExceptOne(difference, _closestItem);
        }
        CanBeCollected = true;
    }

    public void Collect() { 
        if (_closestItem == _sword)
            GameEventSystem.CollectAmmo(1);
        else if (_closestItem == _healthPotion)
            Inventory.CollectPotion(PotionEnum.Red);
        else if (_closestItem == _strengthPotion)
            Inventory.CollectPotion(PotionEnum.Green);
        else if (_closestItem == _speedPotion)
            Inventory.CollectPotion(PotionEnum.Blue);
        CanBeCollected = false;
        Speed = _idleSpeed;
    }
    public RectTransform GetClosestItem()
    {
        return _closestItem;
    }

    private void MoveAllChildrenExceptOne(Vector3 offset, RectTransform exception)
    {
        foreach (RectTransform child in GetComponentsInChildren<RectTransform>())
        {
            if (child == transform || child == exception) continue;
            child.localPosition += offset;
        }
    }
}
