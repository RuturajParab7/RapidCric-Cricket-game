using UnityEngine;
using System;
using System.Collections;
using TMPro;
using System.Drawing;
using UnityEngine.UIElements;


public class BatsmanManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject drawPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CanvasGroup transitionCG;
    [SerializeField] private TextMeshProUGUI transitionScoreText;

    [Header("End game score texts")]
    [SerializeField] private TextMeshProUGUI[] endScoreTexts;

    [Header("Settings")]
    [SerializeField] private Vector2 minMaxbowlingSpeed;
    [SerializeField] private AnimationCurve bowlingSpeedCurve;

    private int currentOver;


    [Header("Events")]
    public static Action onAimingStarted;
    public static Action onBowlingStarted;
    public static Action OnNextOverSet;

    

    IEnumerator Start()
    {
        yield return null;

        winPanel.SetActive(false);
        drawPanel.SetActive(false);
        losePanel.SetActive(false); 

        StartAiming();

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallMissed += BallMissedCallback;
        Ball.onBallHitStump += BallHitStumpCallback;

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallMissed -= BallMissedCallback;
        Ball.onBallHitStump -= BallHitStumpCallback;

        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    private void StartAiming()
    { 
        onAimingStarted?.Invoke();
    }

    public void StartBowling()
    {
       
        onBowlingStarted?.Invoke();
    }

    private void BallHitGroundCallback(Vector3 ballHitPosition)
    {
        currentOver++;

        if (currentOver >= 3)
        {
            //either switch to next game mode
            //OR END THE GAME / COMPARE SCORES
            //Debug.Log("Set next game mode");

            
            if (GameManager.instance.TryStartingNextGameMode())
            {
                UpdateTransitionScore();
                ShowTransitionPanel();
            }
            else
            {
                //game manager returns false means we ended the game
                UpdateEndGameScoreTexts();
            }
            
        }
        else
        {
            SetNextOver();
        }
    }

    private void BallHitStumpCallback()
    {
        currentOver = 2;
        BallHitGroundCallback(Vector3.zero);
    }

    private void SetNextOver()
    {
        StartCoroutine(WaitAndRestart());
        IEnumerator WaitAndRestart()
        {
            yield return new WaitForSeconds(2);

            OnNextOverSet?.Invoke();
            StartAiming();
        }
    }

    private void ShowTransitionPanel()
    {
        LeanTween.alphaCanvas(transitionCG, 1, .5f);
        transitionCG.interactable = true;
        transitionCG.blocksRaycasts = true;
    }

    private void UpdateTransitionScore()
    {
        transitionScoreText.text = "<color #FFFF00>" + ScoreManager.instance.GetPlayerScore() +
            "</color> - <color #0000FF>" + ScoreManager.instance.GetAiScore() + "</color>";
    }


    private void UpdateEndGameScoreTexts()
    {
        for (int i = 0; i < endScoreTexts.Length ; i++)
        {
            endScoreTexts[i].text = "<color #FFFF00>" + ScoreManager.instance.GetPlayerScore() +
            "</color> - <color #0000FF>" + ScoreManager.instance.GetAiScore() + "</color>";
        }
    }

    private void BallMissedCallback()
    {
        BallHitGroundCallback(Vector3.zero);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Win:
                ShowWinPanel(); 
                break;
            case GameState.Lose:
                ShowLosePanel();
                break;
            case GameState.Draw:
                ShowDrawPanel();
                break;
        }
    }


    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void ShowDrawPanel()
    {
        drawPanel.SetActive(true);
    }

    public void NextButtonCallback()
    {
        GameManager.instance.NextButtonCallback();
    }

    public void NextButtonCallback2()
    {
        GameManager.instance.NextButtonCallback2();
    }
}
