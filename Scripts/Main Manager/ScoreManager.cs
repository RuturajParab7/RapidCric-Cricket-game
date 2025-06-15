using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [Header("Settings")]
    private int playerScore;
    private int aiScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;    
        else
            Destroy(gameObject);
    }
    void Start()
    {
        GameManager.onGameSet += ResetScores;
        LocalScoreManager.onScoreCalculated += ScoreCalculatedCallback; 
    }

    private void OnDestroy()
    {
        GameManager.onGameSet -= ResetScores;

        LocalScoreManager.onScoreCalculated -= ScoreCalculatedCallback;

    }

    void Update()
    {
        
    }

    private void ScoreCalculatedCallback(int score)
    {
        if (GameManager.instance.IsBowler())
            aiScore += score;
        else
            playerScore += score;

        Debug.Log("Player score : " + playerScore);
        Debug.Log("Ai score : " + aiScore);
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetAiScore()
    {
        return aiScore;
    }

    public bool IsPlayerWin()
    {
        return playerScore > aiScore;
    }

    public bool IsPlayerLose()
    {
        return playerScore < aiScore;
    }

    public void ResetScores()
    {
        playerScore = 0;
        aiScore = 0;
    }
}
