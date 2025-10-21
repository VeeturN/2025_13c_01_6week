using UnityEngine;

public class TotemBullet : MonoBehaviour
{
    private bool _left;
    [SerializeField] private float _speed = 8f;
    private float _startPositionX;

    public void Init(bool left)
    {
        _left = left;
        _startPositionX = transform.position.x;
        // obrót w poziomie
        Vector3 scale = transform.localScale;
        scale.x = !_left ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3((_left ? _speed : -_speed) * Time.fixedDeltaTime, 0, 0);

        if (Mathf.Abs(transform.position.x - _startPositionX) > 50)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // obrażenia tylko dla gracza
        BasicPlayerMovment player = collision.GetComponent<BasicPlayerMovment>();
        if (player != null)
        {
            player.hit();
            Destroy(gameObject);
            return;
        }

        // niszczy się z czymkolwiek innym
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Pickable") && !collision.CompareTag("Attack"))
        {
            Destroy(gameObject);
        }
    }
}