using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;



public enum GameState {Menu, Bowler, Batsman, Win, Lose, Draw  }
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Settings")]
    private GameState gameState;
    private GameState firstGameState;
    [SerializeField] private CanvasGroup transitionCG;

    [Header("Events")]
    public static Action onGameSet;
    public static Action <GameState> onGameStateChanged;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonCallback()
    {
        int randomStateIndex = Random.Range(0,2);

        if (randomStateIndex == 0)
            firstGameState = GameState.Batsman;
        else
            firstGameState = GameState.Bowler;

       gameState = firstGameState;

        onGameSet?.Invoke();

        Invoke("StartGame",2);

    }

    public void StartGame()
    {
        if (firstGameState == GameState.Bowler)
            StartBowlerMode();
        else
            StartBatsmanMode();
    }

    public bool TryStartingNextGameMode()
    {
        if(gameState == firstGameState)
        {
            LeanTween.delayedCall(2,StartNextGameMode); 
            return true;
        }
        else
        {
            //trigger win draw lose
            Debug.LogWarning("Trigger win loose draw");
            FinishGame();
            return false;
        }
    }

    private void StartNextGameMode()

    {
        //trigger first game mode
        if (firstGameState == GameState.Bowler)
           StartBatsmanMode();
        else
           StartBowlerMode();
    }
    private void StartBowlerMode()
    {
        gameState = GameState.Bowler;
        SceneManager.LoadScene("Bowler");
    }

    private void StartBatsmanMode()
    {
        gameState = GameState.Batsman;
        SceneManager.LoadScene("Batsman");
    }

    private void FinishGame()
    {
        if(ScoreManager.instance.IsPlayerWin())
            //set win state
            SetGameState(GameState.Win);
        else if(ScoreManager.instance.IsPlayerLose())
            //set lose state
            SetGameState(GameState.Lose);
        else
            SetGameState(GameState.Draw);

    }

    public void SetGameState(GameState gamestate)
    {
        this.gameState = gamestate;
        onGameStateChanged?.Invoke(gameState);
    }

    public void NextButtonCallback()
    {
        SetGameState(GameState.Menu);
        SceneManager.LoadScene("Main");
    }

    public void NextButtonCallback2()
    {
        //SetGameState(GameState.Menu);
        SceneManager.LoadScene("BowlerL2");
    }

    public bool IsBowler()
    {
        return gameState == GameState.Bowler;
    }



}
