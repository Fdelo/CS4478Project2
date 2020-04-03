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

  [SerializeField] 
  float jumpForce = 20.0f;
  public bool isgrounded = true;

  SpriteRenderer spriteRenderer;
  Rigidbody2D rb2D; 
  public bool coinCollect = false;


  bool isFacingRight = true;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb2D = GetComponent<Rigidbody2D>();
    
  }

  void Update()
  {
    translation = Input.GetAxisRaw("Horizontal") * speed;   
    isjumping = Input.GetButtonDown("Jump");

    if(isjumping && jumpCount < 2) 
    {      
      jumpCount++;
      rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
      animator.SetBool("IsJumping", true);
    }
  }

  private void FixedUpdate() {   
    isgrounded = false;       
    animator.SetFloat("Speed", Mathf.Abs(translation));      
    if(rb2D.velocity.x < 0 && isFacingRight) {
      Flip();
    } else  if (rb2D.velocity.x > 0 && !isFacingRight){
      Flip();
    }

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
      Destroy(theCollision.gameObject);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "FallDeath")
    {
      GameManager.gameManager.Restart();
    }
    if(other.gameObject.tag == "Finish")
    {
      GameManager.gameManager.gameData.hasWon = true;
    }
  }

  private void Flip() {
      isFacingRight = !isFacingRight;
      Vector3 tmp = transform.localScale;
      tmp.x *= -1;
      transform.localScale = tmp;
  }

  // void OnCollisionExit2D(Collision2D theCollision)
  // {
  //   if (theCollision.gameObject.tag == "Ground")
  //   {
  //     isgrounded = false;
  //   }
  // }

}
