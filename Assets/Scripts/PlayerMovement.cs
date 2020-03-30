using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float speed = 40.0f;
  public Animator animator;

  float translation = 0.0f;
  bool isgrounded = true;
  SpriteRenderer spriteRenderer;
  Rigidbody2D rb2D; 

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb2D = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    translation = Input.GetAxisRaw("Horizontal") * speed;    
    
  }
  private void FixedUpdate() 
  {          
    animator.SetFloat("Speed", Mathf.Abs(translation));
    if(translation < 0) {
      spriteRenderer.flipX = true;
    } else {
      spriteRenderer.flipX = false;
    }

    translation *= Time.fixedDeltaTime;
    transform.Translate(translation,0,0);
    if(Input.GetButtonDown("Jump") && isgrounded) 
    {
      rb2D.AddForce(new Vector2(0.0f, 5.0f), ForceMode2D.Impulse);
      // animator.SetBool("IsJumping", true);
    }
  }


   void OnCollisionEnter2D(Collision2D theCollision)
  {
    
    if (theCollision.gameObject.tag == "Ground")
    {      
      isgrounded = true;
      rb2D.velocity = new Vector2(0.0f, 0.0f);
    }
  }
 

  void OnCollisionExit2D(Collision2D theCollision)
  {
    Debug.Log(isgrounded);
    if (theCollision.gameObject.tag == "Ground")
    {
      Debug.Log(isgrounded);
      isgrounded = false;
    }
  }
}
