using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UIElements;
using TMPro;

public class BowlerManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private BowlerTarget bowlerTarget;
    [SerializeField] private GameObject aimingPanel;
    [SerializeField] private GameObject bowlingPanel;
    [SerializeField] private BowlerPowerSlider powerSlider;
    [SerializeField] private PlayerBowler bowler;

    [Header("Elements")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject drawPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CanvasGroup transitionCG;
    [SerializeField] private TextMeshProUGUI transitionScoreText;
    [SerializeField] private TextMeshProUGUI[] endScoreTexts;

    [Header("Settings")]
    [SerializeField] private Vector2 minMaxbowlingSpeed;
    [SerializeField] private AnimationCurve bowlingSpeedCurve;

    private int currentOver;


    [Header("Events")]
    public static Action onAimingStarted;
    public static Action onBowlingStarted;
    public static Action OnNextOverSet;



    
    void Start()
    {
        StartAiming();

        BowlerPowerSlider.onPowerSliderstopped += PowerSliderStoppedCallback;

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallMissed += BallMissedCallback;
        Ball.onBallHitStump += BallHitStumpCallback;

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        BowlerPowerSlider.onPowerSliderstopped -= PowerSliderStoppedCallback;

        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallMissed -= BallMissedCallback;
        Ball.onBallHitStump -= BallHitStumpCallback;

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartAiming()
    {
        //1.enable movement of bowler target
        bowlerTarget.EnableMovement();

        //2.Hide power slider
        aimingPanel.SetActive(true);
        bowlingPanel.SetActive(false);

        //3.Enable the aiming camera


        //4.Disable the aiming camera
        onAimingStarted?.Invoke();

    }

    public void StartBowling()
    {
        //1.disable the movement of bowler target
        bowlerTarget.DisableMovement();

        //2.show the power slider
        bowlingPanel.SetActive(true);
        aimingPanel.SetActive(false);

        onBowlingStarted?.Invoke();

        //3.enable movement of slider
        powerSlider.StartMoving();
    }

    private void PowerSliderStoppedCallback(float power)
    {
        float lerp = bowlingSpeedCurve.Evaluate(power);
        float bowlingSpeed = Mathf.Lerp(minMaxbowlingSpeed.x,minMaxbowlingSpeed.y,lerp);

        bowler.StartRunning(bowlingSpeed);

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

    private void UpdateEndGameScoreTexts()
    {
        for (int i = 0; i < endScoreTexts.Length; i++)
        {
            endScoreTexts[i].text = "<color #FFFF00>" + ScoreManager.instance.GetPlayerScore() +
            "</color> - <color #0000FF>" + ScoreManager.instance.GetAiScore() + "</color>";
        }
    }

    private void UpdateTransitionScore()
    {
        transitionScoreText.text = "<color #FFFF00>" + ScoreManager.instance.GetPlayerScore() +
            "</color> - <color #0000FF>" + ScoreManager.instance.GetAiScore() + "</color>";
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

    private void BallMissedCallback()
    {
        BallHitGroundCallback(Vector3.zero);
    }


    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
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
