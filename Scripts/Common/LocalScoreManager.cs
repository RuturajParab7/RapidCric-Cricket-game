using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class LocalScoreManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI[] scoreTexts;

    [Header("Settings")]
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float secondRingRadius;
    private int currentOver;

    [Header("Events")]
    public static Action<int> onScoreCalculated;

    void Start()
    {
        ClearTexts();

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallMissed += BallMissedCallback;
    }

    private void OnDestroy()
    {
        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallMissed -= BallMissedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BallHitGroundCallback(Vector3 ballHitPosition)
    {
        //1.Calculate score that we will add to the batsman  //42  70  
        float ballDistance = ballHitPosition.magnitude;

        int score = 2;

        if (ballDistance > firstRingRadius)
            score += 2;
        if (ballDistance > secondRingRadius)
            score += 2;

        onScoreCalculated?.Invoke(score);

        scoreTexts[currentOver].text = score.ToString();

        currentOver++;
    }

    private void BallMissedCallback()
    {
        scoreTexts[currentOver].text = "0";
        currentOver++;
        onScoreCalculated?.Invoke(0);

    }


    private void ClearTexts()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
            scoreTexts[i].text = "";
    }
        
}
