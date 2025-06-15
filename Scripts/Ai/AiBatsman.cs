//using UnityEditor.Experimental.GraphView;
using System;
using System.Collections;
using UnityEngine;

public class AiBatsman : MonoBehaviour
{
    enum State {Moving,Hitting }

    [Header("Elements")]
    [SerializeField] private BowlerTarget bowlerTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider batcollider;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask ballMask;
    [SerializeField] private Vector2 minMaxHitVelovity;
    [SerializeField] private float maxHitDuration;
    private State state;
    private bool canDetectHits;
    private float hitTimer;


    [Header("Events")]
    public static Action<Transform> onBallHit;
    void Start()
    {
        state = State.Moving;
        PlayerBowler.onBallThrown += BallThrownCallback;

        BowlerManager.OnNextOverSet += Restart;
    }

    private void OnDestroy()
    {
        PlayerBowler.onBallThrown -= BallThrownCallback;
        BowlerManager.OnNextOverSet -= Restart;

    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch(state)
        {
            case State.Moving:
                Move();
                break;

            case State.Hitting: 
                if (canDetectHits) 
                    CheckForHits();
                break;
        }
    }

    private void Move()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = GetTargetX();

        //ftargetPosition.x = Mathf.Clamp(targetPosition.x, 17.14f, 16.67f);

        //calc how far are we from target X
        float difference = targetPosition.x - transform.position.x;

        if (difference == 0)
            animator.Play("Idle");
        else if (difference > 0)
            animator.Play("Left");
        else
            animator.Play("Right");

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        //transform.position = position;

    }

    private void BallThrownCallback(float ballFlightDuration)
    {
        state = State.Hitting;

        StartCoroutine(WaitAndHitCoroutine());

        IEnumerator WaitAndHitCoroutine()
        {
            float bestDelay = ballFlightDuration - .8f;

            //float delay = Random.Range(bestDelay - .2f, bestDelay + .2f);  
            float delay = UnityEngine.Random.Range(bestDelay - .4f, bestDelay + .3f);  //bestDelay - .2f, bestDelay + .2f

            yield return new WaitForSeconds(delay); //best the delay best the hit speed
            animator.Play("Hit");
        }

        
    }

    public void StartDetectingHits()
    {
        canDetectHits = true;
        hitTimer = 0;
    }

    private void CheckForHits()
    {
        Vector3 center = batcollider.transform.TransformPoint(batcollider.center);
        Vector3 halfExtents = 1.5f * batcollider.size / 2;
        Quaternion rotation = batcollider.transform.rotation;

        Collider[] detectedBalls = Physics.OverlapBox(center,halfExtents,rotation,ballMask);

        for (int i = 0; i < detectedBalls.Length; i++) 
        {
            BallDetectedCallback(detectedBalls[i]);
            return;
        }

        hitTimer += Time.deltaTime;
    }

    private void BallDetectedCallback(Collider ballCollider)
    {
        canDetectHits = false;

        ShootBall(ballCollider.transform);
    }

    private void ShootBall(Transform ball)
    {
        //compare the hit timer with max duration
        //if hitTimer=0 -> maxHitVelocity 
        //if hittimer > maxhitDuration -> minvelocity

        float lerp = Mathf.Clamp01(hitTimer / maxHitDuration);
        float hitVelocity = Mathf.Lerp(minMaxHitVelovity.y,minMaxHitVelovity.x,lerp);

        Vector3 hitVelocityVector = (Vector3.back + Vector3.up + Vector3.right * UnityEngine.Random.Range(-1f, 1f)) * hitVelocity;

        ball.GetComponent<Ball>().GetHitByBat(hitVelocityVector);
        //ball.GetComponent<Rigidbody>().velocity = 

        onBallHit?.Invoke(ball);
    }

    private float GetTargetX()
    {
        Vector3 bowlerShootPosition = new Vector3(-1, 0, -18.7f);
        Vector3 shootDirection= (bowlerTarget.transform.position - bowlerShootPosition).normalized;
        float shootAngle = Vector3.Angle(Vector3.right, shootDirection);

        float bc = transform.position.z - bowlerShootPosition.z;
        float ab = bc/Mathf.Sin(shootAngle * Mathf.Deg2Rad);

        Vector3 targetAiPosition = bowlerShootPosition + shootDirection.normalized * ab;

        return targetAiPosition.x ;
    }

    private void Restart()
    {
        state = State.Moving;
    }
}
