using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
  public float speed = 200.0f;
  public Animator animator;

  Camera main_cam;
  
  float translation = 0.0f;
  
  private bool isjumping = false;
  int jumpCount = 0;
  [SerializeField]
  float jumpForce = 20.0f;

  SpriteRenderer spriteRenderer;
  Rigidbody2D rb2D; 

  bool isgrounded = true;


  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb2D = GetComponent<Rigidbody2D>();
  }
  private void Start() {
    main_cam = Camera.main;
  }

  void Update()
  {
    translation = Input.GetAxisRaw("Horizontal") * speed;   
    isjumping = Input.GetButtonDown("Jump");
        
    if(isjumping && jumpCount < 2) 
    {      
      jumpCount++;
      // rb2D.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
      rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
      animator.SetBool("IsJumping", true);
    }
  }

  private void FixedUpdate() {          
    animator.SetFloat("Speed", Mathf.Abs(translation));      
    if(translation < 0) {
      spriteRenderer.flipX = true;
    } else {
      spriteRenderer.flipX = false;
    }


    translation *= Time.fixedDeltaTime;

    rb2D.velocity = new Vector2(translation, rb2D.velocity.y);

    main_cam.transform.position = new Vector3(transform.position.x, transform.position.y, main_cam.transform.position.z);
    
  }


   void OnCollisionEnter2D(Collision2D theCollision)
  {
    if (theCollision.gameObject.tag == "Ground")
    {      
      animator.SetBool("IsJumping", false);    
      isgrounded = true;
      jumpCount = 0;
    }
  }
 

  void OnCollisionExit2D(Collision2D theCollision)
  {
    if (theCollision.gameObject.tag == "Ground")
    {
      isgrounded = false;
    }
  }
}
