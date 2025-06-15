using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerBatsman : MonoBehaviour
{
    enum State { Moving, Hitting }

    [Header("Elements")]
    
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider batcollider;

    [Header("Settings")]
    [SerializeField] private LayerMask ballMask;
    [SerializeField] private Vector2 minMaxHitVelovity;
    [SerializeField] private float maxHitDuration;
    private State state;

    private bool canDetectHits;
    private float hitTimer;

    [Header(" Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 minMaxX;
    private Vector3 clickedPosition;
    private Vector3 clickedTargetPosition;



    [Header("Events")]
    public static Action<Transform> onBallHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Moving;
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
            case State.Moving:
                ManageControl();
                break;

            case State.Hitting:
                if (canDetectHits)
                    CheckForHits();
                break;
        }
    }


    private void Move(float targetX)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = targetX;

        

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

    public void StartDetectingHits()
    {
        canDetectHits = true;
        hitTimer = 0;
    }

    public void StopDetectingHits()
    {
        canDetectHits = false;
        state = State.Moving;
    }

    private void CheckForHits()
    {
        Vector3 center = batcollider.transform.TransformPoint(batcollider.center);
        Vector3 halfExtents = 1.5f * batcollider.size / 2;
        Quaternion rotation = batcollider.transform.rotation;

        Collider[] detectedBalls = Physics.OverlapBox(center, halfExtents, rotation, ballMask);

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
        float hitVelocity = Mathf.Lerp(minMaxHitVelovity.y, minMaxHitVelovity.x, lerp);

        Vector3 hitVelocityVector = (Vector3.back + Vector3.up + Vector3.right * UnityEngine.Random.Range(-1f, 1f)) * hitVelocity;

        ball.GetComponent<Ball>().GetHitByBat(hitVelocityVector);
        //ball.GetComponent<Rigidbody>().velocity = 

        onBallHit?.Invoke(ball);
    }

    

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedPosition = Input.mousePosition;
            clickedTargetPosition = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 difference = Input.mousePosition - clickedPosition;

            difference.x /= Screen.width;

            float targetX = clickedTargetPosition.x - (difference.x * moveSpeed);

            Vector3 targetPosition = clickedTargetPosition + Vector3.right * (difference.x * moveSpeed);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minMaxX.x, minMaxX.y);
            

            transform.position = targetPosition;
            Move(targetX);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Move(transform.position.x);
        }
    }

    public void HitButtonCallback()
    {
        state = State.Hitting;

        animator.Play("Player Hit");
    }

    private void Restart()
    {

    }
}
