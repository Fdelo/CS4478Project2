using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

  public static GameManager gameManager;
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
  bool isrunning = false;
  bool changeScore = false;

  string[] levels = {
    "Level1", 
    "Level2"
  };
  int currentLevel = 0;


  private void Awake() {
    gameManager = this;
    DontDestroyOnLoad(gameManager);
    gameData = new GameData();
    SceneManager.sceneLoaded += OnSceneLoad;
  }

  private void Start() {
    // score = GameObject.Find("Score1").GetComponent<Text>();
  }

  private void Update() {
    // if(changeScore) {
    //   while(score.name != ("Score" + (currentLevel + 1))) score = GameObject.Find("Score" + (currentLevel + 1)).GetComponent<Text>();
    //   changeScore = false;
    // }
    // if(score != null){
    //   score.text = "Score: " + gameData.coins;
    // }
    

    if(gameData.hasWon)
    {
      gameData.hasWon = false;
      if(!isrunning) StartCoroutine(NextLevel());      
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
    if(scene.name != "Main") changeScore = true;
  }

  public void Restart()
  {
    string tmp = SceneManager.GetActiveScene().name;
    SceneManager.UnloadSceneAsync(tmp);
    SceneManager.LoadSceneAsync(tmp);
  }

  public void Exit()
  {
    Application.Quit();
  }
}
