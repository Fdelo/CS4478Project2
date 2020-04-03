using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

  bool coinCollect = false;

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "Player" && !coinCollect)
    {
      coinCollect = true;
      GameManager.gameManager.gameData.coins += 1;
     
      Destroy(this.gameObject);      
    }
  }

}
