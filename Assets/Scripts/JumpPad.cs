using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 20f;
    private Animator jumpPadAnim;

    [SerializeField] private AudioSource jumpPadSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;

    private void Start()
    {
      jumpPadAnim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                jumpPadAnim.SetTrigger("jump");
                jumpPadSoundEffect.Play();
                jumpSoundEffect.Play();
            }
        }
    }
}
