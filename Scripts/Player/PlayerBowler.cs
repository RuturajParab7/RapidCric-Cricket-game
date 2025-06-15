using UnityEngine;
using System;

public class PlayerBowler : MonoBehaviour
{
    public enum State { Idle, Aiming, Running, Bowling }
    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fakeBall;
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private GameObject ballTarget;
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runDuration;
    //[SerializeField] private float flightDurationMultiplier; //remove if needed fDM

    private float runTimer;

    private State state;
    private float bowlingSpeed;
    private Vector3 initialPosition;

    [Header("Events")]
    public static Action<float> onBallThrown;



    void Start()
    {
        state = State.Idle;
        initialPosition = transform.position;

        BowlerManager.OnNextOverSet += Restart; 

    }

    private void OnDestroy()
    {
        BowlerManager.OnNextOverSet -= Restart;
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
                break;

            case State.Running:
                Run();
                break;

            case State.Bowling:
                break;
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

        float duration =  distance / velocity; //removed fDM * 

        
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
