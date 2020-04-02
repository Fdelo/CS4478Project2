﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  
  public float speed = 200.0f;
  public Animator animator;
  
  float translation = 0.0f;
  
  private bool isjumping = false;
  int jumpCount = 0;
  [SerializeField]
  float jumpForce = 20.0f;

  SpriteRenderer spriteRenderer;
  Rigidbody2D rb2D; 
  GameManager gameManager;
  public bool coinCollect = false;

  bool isgrounded = true;
  bool isFacingLeft = true;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb2D = GetComponent<Rigidbody2D>();
    gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
    if(GameObject.FindGameObjectsWithTag("Coin").Length == 0) gameManager.gameData.hasWon = true;
  }

  private void FixedUpdate() {          
    animator.SetFloat("Speed", Mathf.Abs(translation));      
    if(rb2D.velocity.x > 0 && isFacingLeft) {
      isFacingLeft = !isFacingLeft;
      Vector3 tmp = transform.localScale;
      tmp.x *= -1;
      transform.localScale = tmp;
    } else  if (rb2D.velocity.x < 0 && !isFacingLeft){
      isFacingLeft = !isFacingLeft;
      Vector3 tmp = transform.localScale;
      tmp.x *= -1;
      transform.localScale = tmp;
    }

    translation *= Time.fixedDeltaTime;
    rb2D.velocity = new Vector2(translation, rb2D.velocity.y);
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
   
  private void OnTriggerEnter2D(Collider2D other) {   
    if(!coinCollect && other.gameObject.tag == "Coin")
    {
      gameManager.gameData.coins += 1;
      coinCollect = true;
      Destroy(other.gameObject);      
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if(coinCollect && other.gameObject.tag == "Coin")
    {
      coinCollect = false;   
    }
  }
}
