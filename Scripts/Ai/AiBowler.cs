using System;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;


public class AiBowler : MonoBehaviour
{
    public enum State { Idle, Aiming, Running, Bowling }
    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fakeBall;
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private GameObject ballTarget;
    [SerializeField] private BowlerTarget bowlerTarget;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runDuration;
    //[SerializeField] private float flightDurationMultiplier; //remove if needed fDM

    private float runTimer;

    private State state;
    private float bowlingSpeed;
    private Vector3 initialPosition;
    private float aimingTimer;

    [Header("Events")]
    public static Action<float> onBallThrown;



    void Start()
    {
        state = State.Idle;
        initialPosition = transform.position;

        BatsmanManager.onAimingStarted += StartAiming;

        BatsmanManager.OnNextOverSet += Restart;

    }

    private void OnDestroy()
    {
        BatsmanManager.onAimingStarted -= StartAiming;
        BatsmanManager.OnNextOverSet -= Restart;
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Aiming:
                Aim();
                break;

            case State.Running:
                Run();
                break;

            case State.Bowling:
                break;
        }
    }

 private void StartAiming()
    {
        state = State.Aiming;
        aimingTimer = 0;
    }

    private void Aim()
    {
        // implement this method
        float x = Mathf.PerlinNoise(0, Time.time + 36);
        float y = Mathf.PerlinNoise(0, Time.time * 2);

        Vector2 targetPosition = new Vector2(x, y);

        bowlerTarget.Move(targetPosition);

        aimingTimer += Time.deltaTime; 

        if(aimingTimer > 2)
        {
            StartRunning(80);
        }
    }

    public void StartRunning(float bowlingSpeed)
    {
        this.bowlingSpeed = bowlingSpeed;
        state = State.Running;

        runTimer = 0;
        animator.SetInteger("State", 1);
    }


    private void Run()
    {
        transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
        runTimer += Time.deltaTime;

        if (runTimer >= runDuration)
            StartBowling();

    }

    private void StartBowling()
    {
        state = State.Bowling;
        animator.Play("Throw");
        animator.SetInteger("State", 2);


    }

    public void ThrowBall()
    {
        fakeBall.SetActive(false);

        Vector3 from = fakeBall.transform.position;
        Vector3 to = ballTarget.transform.position;

        //calculate duration of flight depending on bowling speed
        //velocity = distance/time;

        float distance = Vector3.Distance(from, to);
        float velocity = bowlingSpeed / 3.6f;

        float duration = distance / velocity; //removed fDM * 


        ballLauncher.LaunchBall(from, to, duration);

        onBallThrown?.Invoke(duration);


    }

    private void Restart()
    {
        state = State.Idle;
        transform.position = initialPosition;

        animator.SetInteger("State", 0);
        animator.Play("Idle");

        fakeBall.SetActive(true);
    }
}
