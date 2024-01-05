using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed = 5f;
  public LayerMask collisionLayers;

  private Rigidbody2D rb;
  private float direction;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  public void Initialize(float newDirection)
  {
    if (rb == null)
    {
      return;
    }

    direction = newDirection;

    if (direction > 0)
    {
      transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    else
    {
      transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    Move();
  }

  private void Move()
  {
    if (rb == null)
    {
      return;
    }

    rb.velocity = new Vector2(speed * direction, 0);
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
    {
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
    {
      Destroy(gameObject);
    }
  }
}
