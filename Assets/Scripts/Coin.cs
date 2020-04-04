using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

  bool coinCollect = false;
  AudioSource sfx;

  private void Start() {
    sfx = GetComponent<AudioSource>();
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "Player" && !coinCollect)
    {
      AudioSource.PlayClipAtPoint(sfx.clip, transform.position);
      coinCollect = true;
      GameManager.gameManager.coins += 1;

      // Destroy(this.gameObject);      
      this.gameObject.SetActive(false);
    }
  }

}
