using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  
  public Animator animator;
  public Transform groundCheck;

  float translation = 0.0f;
  public float speed = 200.0f;
  private bool isjumping = false;
  int jumpCount = 0;
  float jumpForce = 12.0f;
  bool isgrounded = true;
  bool wasGrounded = false;
  bool isFacingRight = true;

  SpriteRenderer spriteRenderer;
  Rigidbody2D rb2D; 
  
  AudioSource sfx;

  public float health = 100.0f;
  bool hitCooldown = true;
  float cdTime = 0.0f;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb2D = GetComponent<Rigidbody2D>();    
    GameManager.gameManager.playerHealth = health;
    sfx = GetComponent<AudioSource>();
  }

  void Update()
  {
    translation = Input.GetAxisRaw("Horizontal") * speed;   
    isjumping = Input.GetButtonDown("Jump");

    // * allow double jumping
    if(isjumping && jumpCount < 1) 
    {      
      jumpCount++;
      rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
      animator.SetBool("IsJumping", true);
    }

    // * player can only get hit once every 2 seconds
    if(Time.time - cdTime > 2.0f)
    {
      animator.SetBool("IsHit", false);
      hitCooldown = false;
    }

    if(health == 0.0f)
    {
      health = 100.0f;
      GameManager.gameManager.playerHealth = health;
      GameManager.gameManager.lives -= 1;
      GameManager.gameManager.RestartLevel();
    }
  }

  private void FixedUpdate() {     
    wasGrounded = isgrounded;
    isgrounded = false;
    animator.SetFloat("Speed", Mathf.Abs(translation));      
    if(rb2D.velocity.x < 0 && isFacingRight) 
    {
      Flip();
    } 
    else  if (rb2D.velocity.x > 0 && !isFacingRight)
    {
      Flip();
    }

    // * ground check
    Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, .2f);
    for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{        
				isgrounded = true;
        jumpCount = 0;
			}
		}
    translation *= Time.fixedDeltaTime;
    rb2D.velocity = new Vector2(translation, rb2D.velocity.y);
  }


  void OnCollisionEnter2D(Collision2D theCollision)
  {
    
    if (isgrounded)
    {  
      animator.SetBool("IsJumping", false);          
    }

    if(theCollision.gameObject.tag == "Enemy")
    {   
      // * check if player lands on enemy
      if(groundCheck.position.y > theCollision.transform.position.y)
      {
        sfx.Play(); 
        Destroy(theCollision.gameObject);    
      }
      else if(!hitCooldown)
      {
        sfx.Play();
        Damage(5.0f);
      }             
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "FallDeath")
    {
      // * prevents double triggers      
      other.gameObject.SetActive(false);
      GameManager.gameManager.lives -= 1;
      GameManager.gameManager.RestartLevel();
    }
    if(other.gameObject.tag == "Finish")
    {
      GameManager.gameManager.beatLevel = true;
    }
  }

  private void Flip() {
    isFacingRight = !isFacingRight;
    Vector3 tmp = transform.localScale;
    tmp.x *= -1;
    transform.localScale = tmp;
  }

  private void Damage(float amount)
  {
    health -= amount;
    GameManager.gameManager.playerHealth = health;
    animator.SetBool("IsHit", true);
    hitCooldown = true;
    cdTime = Time.time;
  }

}
