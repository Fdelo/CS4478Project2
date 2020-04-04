using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

  public static GameManager gameManager;
  public int coins { get; set; }
  public bool beatLevel { get; set; }
  public int lives { get; set; }  
  public float playerHealth;
  bool isrunning = false;
  bool changeScore = false;
  Canvas dispCanvas;
  Canvas endCanvas;
  
  TextMeshProUGUI[] textboxes;

  string[] levels = {
    "Level1", 
    "Level2"
  };
  int currentLevel = 0;


  private void Awake() {
    // * use this technique so the game manager can easily be accessed from other scripts
    gameManager = this;
    // * this method is used a couple times on items we do not want destroyed
    DontDestroyOnLoad(gameManager);
    coins = 0;
    beatLevel = false;
    lives = 3;    
    SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);    
    SceneManager.sceneLoaded += OnSceneLoad;
    dispCanvas = GameObject.Find("DisplayCanvas").GetComponent<Canvas>();
    endCanvas = GameObject.Find("EndScreenCanvas").GetComponent<Canvas>();
    textboxes = dispCanvas.GetComponentsInChildren<TextMeshProUGUI>();
    DontDestroyOnLoad(dispCanvas);
    endCanvas.gameObject.SetActive(false);
    DontDestroyOnLoad(endCanvas);
     
  }

  private void Update() {

    // * continually set UI to update based on the correcet info
    textboxes[0].text = "Health: " + playerHealth;
    textboxes[1].text = "Lives: " + lives;
    textboxes[2].text = "Score: " + coins;
    
    if(beatLevel)
    {
      // * check for last level
      if(levels[currentLevel] == "Level2")
      {
        GameOver("You Win");
      }
      else
      {
        beatLevel = false;
        if(!isrunning) StartCoroutine(NextLevel());
      }

    }
    // * Show game over screen
    if(lives == 0)
    {
      GameOver("You lose");
    }

    // * should bring up a pause screen, does not currently work
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if(endCanvas.enabled)
      {
        Time.timeScale = 1;
        dispCanvas.gameObject.SetActive(true);
        endCanvas.gameObject.SetActive(false);
      }
      else{
        Time.timeScale = 0;
        dispCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(true);
      }
      
    }  
  }

  IEnumerator NextLevel()
  {
    isrunning = true;

    currentLevel += 1;
    // The Application loads the Scene in the background at the same time as the current Scene.
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levels[currentLevel], LoadSceneMode.Additive);

    // Wait until the last operation fully loads to return anything
    while (!asyncLoad.isDone)
    {
        yield return null;
    }
    
    // Unload the previous Scene
    SceneManager.UnloadSceneAsync(levels[currentLevel-1]);
    isrunning = false;
  }

  void OnSceneLoad(Scene scene, LoadSceneMode mode) {    
    SceneManager.SetActiveScene(scene);
  }

  public void RestartLevel()
  {
    
    string tmp = SceneManager.GetActiveScene().name;    
    SceneManager.LoadSceneAsync(tmp);
    
  }

  public void GameOver(string text)
  {
    coins = 0;
    beatLevel = false;
    lives = 3;  
    dispCanvas.gameObject.SetActive(false);
    endCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
    Time.timeScale = 0;
    endCanvas.gameObject.SetActive(true);
  }

  public void Restart() 
  {
    Time.timeScale = 1;
    endCanvas.gameObject.SetActive(false);
    dispCanvas.gameObject.SetActive(true);
    SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);    
  }

  public void Exit()
  {
    Application.Quit();
  }
}
