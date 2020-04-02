using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

  public class GameData
  {
    public int coins { get; set; }
    public bool hasWon { get; set; }

    public GameData()
    {
      coins = 0;
      hasWon = false;
    }
  }

  public GameData gameData;
  Text score;


  private void Awake() {
    gameData = new GameData();    
    score = GameObject.Find("Score").GetComponent<Text>();
  }

  private void Update() {
    score.text = "Score: " + gameData.coins;
  }

  public void Exit()
  {
    Application.Quit();
  }
}




  // private void Awake() {
  //   // * Find the canvas and store it in a variable
  //   gameOverScreen = FindObjectOfType<Canvas>();
    
  //   // * turn it off
  //   gameOverScreen.enabled = false;
  // }

  // public void GameOver () {

  //   if(!gameEnded)
  //   {
  //     // * Turn on the screen
  //     gameEnded = true;
  //     gameOverScreen.enabled = true;
  //   } 
  // }

//   public void Restart()
//   {
//     // * Onclick function for the button, use scenemanager to restart the scene
//     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//   }


//   private void Update() {
//     if (Input.GetKeyDown(KeyCode.Escape))
//     {
//       if(gameOverScreen.enabled)
//       {
//         gameOverScreen.enabled = false;
//       }
//       else{
//         gameOverScreen.enabled = true;
//       }
      
//     }
//   }
// }
