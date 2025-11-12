using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _goLength;
    [SerializeField] private float _speed;
    [SerializeField] BoxCollider2D _noReturn;
    [SerializeField] GameObject _sail;
    private Animator _sailAnimator;
    private BoxCollider2D _col;
    private float _halfHeight;
    private float _halfWidth;
    private float _startPosY;
    private float _startPosX;
    private bool _up=true;
    private bool _goToPoint = false;
    EffectsManager _effectsManager;
    void Awake()
    {
        _effectsManager = GameObject.FindGameObjectWithTag("EffectsManager").GetComponent<EffectsManager>();
        _sailAnimator = _sail.GetComponent<Animator>();
        _noReturn.gameObject.SetActive(false);
        _col = GetComponent<BoxCollider2D>();
        _startPosY = transform.position.y;
        _startPosX = transform.position.x;
        _halfWidth = _col.bounds.extents.x;
        _halfHeight = _col.bounds.extents.y;
    }
    void Update()
    {
        if (_up)
        {
            transform.position += Vector3.up * _halfWidth * Time.deltaTime/4;
            if (transform.position.y > _startPosY + _halfWidth / 30) {
                _up = false;
            }
        }
        else
        {
            transform.position -= Vector3.up * _halfWidth * Time.deltaTime/4;
            if (transform.position.y < _startPosY - _halfWidth / 30)
            {
                _up = true;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_goToPoint)
        {
            if (collision.transform.position.y < transform.position.y + _halfHeight / 2)
                return;

            _goToPoint = true;
            BasicPlayerMovment player = collision.gameObject.GetComponent<BasicPlayerMovment>();
            player.StopMovement();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            EffectScript[] waterSplashes = _effectsManager.WaterSplashEffect(
                transform.position+Vector3.right*_halfWidth/3*2 + Vector3.up * _halfHeight / 2,
                transform.position + Vector3.left * _halfWidth + Vector3.up* _halfHeight / 2);
            foreach (EffectScript effect in waterSplashes)
                effect.transform.SetParent(transform);
            StartCoroutine(TripCoroutine(player,waterSplashes));
        }
    }
    private IEnumerator MovePlayerToTheMiddleCoroutine(BasicPlayerMovment player)
    {
        player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x) * Mathf.Sign(transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
        Vector3 target = new Vector3(-0.25f, 0.5f, 0f);
        while (player.transform.localPosition != target)
        {
            player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition, target, 2f * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator TripCoroutine(BasicPlayerMovment player, EffectScript[] waterSplashes)
    {
        _sailAnimator.SetBool("isGoing", true);
        player.transform.SetParent(transform);
        yield return StartCoroutine(MovePlayerToTheMiddleCoroutine(player));
        Vector3 stayingPos = player.transform.localPosition;
        while (transform.position.x < _startPosX + _goLength)
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
            player.transform.localPosition = stayingPos;
            yield return null;
        }
        foreach(EffectScript effect in waterSplashes)
            effect.DestroySelf();
        _sailAnimator.SetBool("isGoing", false);
        player.ResumeMovement();
        _noReturn.gameObject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _goToPoint)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

}
