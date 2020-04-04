using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

  public float patrolDist = 2.0f;

  float moveSpeed = -1.5f;
  float originalPosition;

  private void Start() {
    originalPosition = transform.position.x;
  }

  private void Update() {
    if(Mathf.Abs(transform.position.x - originalPosition) >= patrolDist) 
    {      
      Vector3 tmp = transform.localScale;
      tmp.x *= -1;
      transform.localScale = tmp;
      moveSpeed *= -1;

    }

    transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
  }

}
