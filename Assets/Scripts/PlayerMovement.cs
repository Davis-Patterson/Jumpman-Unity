using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private SpriteRenderer sprite;
  private Animator anim;

  [SerializeField] private LayerMask jumpableGround;

  private float dirX = 0f;
  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private float jumpForce = 14f;

  [SerializeField] private float gravityScale = 3f;

  public enum MovementState { idle, running, jumping, falling, doublejump, wallslide }

  [SerializeField] private AudioSource jumpSoundEffect;
  [SerializeField] private AudioSource doubleJumpSoundEffect;

  private bool canDoubleJump = true;

  private bool isWallSliding;
  private float wallSlidingSpeed = 1.2f;

  [SerializeField] private Transform wallCheck;
  [SerializeField] private Vector2 wallCheckOffset;
  [SerializeField] private LayerMask wallLayer;

  private bool isWallJumping;
  private float wallJumpTime = 0.3f;
  private Vector2 wallJumpForce = new Vector2(7f, 14f);
  private float wallJumpDirection;

  private bool disableHorizontalMovement = false;


  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<BoxCollider2D>();
    sprite = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    rb.gravityScale = gravityScale;
  }

  private void Update()
  {
    if (!isWallJumping)
    {
      dirX = Input.GetAxisRaw("Horizontal");
      rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    if (Input.GetButtonDown("Jump"))
    {
      if (IsGrounded())
      {
        Jump();
        canDoubleJump = true;
      }
      else if (canDoubleJump)
      {
        DoubleJump();
      }
    }

    WallSlide();
    UpdateAnimationState();

    if (isWallSliding && Input.GetButtonDown("Jump"))
    {
      WallJump();
    }
  }

  private void FixedUpdate()
  {
    if (!isWallJumping && !disableHorizontalMovement)
    {
      rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }
  }



  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("JumpPad") || collision.gameObject.CompareTag("Enemy"))
    {
      ResetDoubleJump();
    }
  }


  private void Jump()
  {
    jumpSoundEffect.Play();
    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    ResetDoubleJump();
  }


  private void DoubleJump()
  {
    doubleJumpSoundEffect.Play();
    rb.velocity = new Vector2(rb.velocity.x, jumpForce / 1.2f);
    canDoubleJump = false;
    SetMovementState(MovementState.doublejump);
  }

  private void ResetDoubleJump()
  {
    canDoubleJump = true;
  }

  private bool IsGrounded()
  {
    return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
  }

  private bool IsWalled()
  {
    Vector2 startPosition = new Vector2(transform.position.x, transform.position.y) + (sprite.flipX ? Vector2.left : Vector2.right) * 0.1f; // Slight offset from the center
    Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
    float rayLength = Mathf.Abs(wallCheckOffset.x);

    RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, rayLength, wallLayer);

    return hit.collider != null;
  }


  private void WallSlide()
  {
    if (!isWallJumping)
    {
      bool isWallPresent = IsWalled();
      bool isPressingTowardsWall = (sprite.flipX && dirX < 0) || (!sprite.flipX && dirX > 0);

      if (isWallPresent && !IsGrounded() && isPressingTowardsWall)
      {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
      }
      else
      {
        isWallSliding = false;
      }
    }
  }

  private void WallJump()
  {
    isWallSliding = false;
    isWallJumping = true;
    wallJumpDirection = sprite.flipX ? 1 : -1;
    rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
    disableHorizontalMovement = true;

    canDoubleJump = true;

    Invoke(nameof(ResetWallJump), wallJumpTime);
  }

  private void ResetWallJump()
  {
    isWallJumping = false;
    disableHorizontalMovement = false;
  }

  private void UpdateAnimationState()
  {
    if (!canDoubleJump && rb.velocity.y > .1f)
    {
      return;
    }

    MovementState state;

    if (isWallSliding)
    {
      state = MovementState.wallslide;
    }
    else if (isWallJumping)
    {
      state = MovementState.jumping;
    }
    else if (dirX > 0f)
    {
      state = MovementState.running;
      sprite.flipX = false;
      wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheckOffset.x), wallCheckOffset.y, 0);
    }
    else if (dirX < 0f)
    {
      state = MovementState.running;
      sprite.flipX = true;
      wallCheck.localPosition = new Vector3(-Mathf.Abs(wallCheckOffset.x), wallCheckOffset.y, 0);
    }
    else
    {
      state = MovementState.idle;
    }

    if (!isWallSliding && !isWallJumping)
    {
      if (rb.velocity.y > .1f)
      {
        state = MovementState.jumping;
      }
      else if (rb.velocity.y < -.1f)
      {
        state = MovementState.falling;
      }
    }

    anim.SetInteger("state", (int)state);
  }

  private void SetMovementState(MovementState newState)
  {
    anim.SetInteger("state", (int)newState);
  }

  public void ResetState()
  {
    isWallJumping = false;
    isWallSliding = false;
    anim.SetInteger("state", (int)MovementState.idle);
  }
}