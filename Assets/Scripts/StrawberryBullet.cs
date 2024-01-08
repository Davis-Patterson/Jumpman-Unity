using UnityEngine;

public class StrawberryBullet : MonoBehaviour
{
  public float speed = 10f;
  public LayerMask collisionLayers;
  public LayerMask groundLayers;

  private Rigidbody2D rb;
  private float direction;
  private bool isBouncing = false;
  private float bounceForce = 5f;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  public void Initialize(float newDirection)
  {
    direction = newDirection;
    Debug.Log("Bullet direction: " + direction);
    Move();
  }

  private void Move()
  {
    rb.velocity = new Vector2(speed * direction, 0);
  }

  void Update()
  {
    if (isBouncing)
    {
      rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    if (Mathf.Approximately(rb.velocity.x, 0))
    {
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (((1 << collision.gameObject.layer) & groundLayers) != 0)
    {
      rb.velocity = new Vector2(speed * direction, bounceForce);
      isBouncing = true;
    }
    else if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
    {
      Destroy(gameObject);
    }
  }
}
