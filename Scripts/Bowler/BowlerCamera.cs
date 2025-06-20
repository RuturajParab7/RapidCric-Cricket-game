using UnityEngine;




public class BowlerCamera : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject aimingCamera;
    [SerializeField] private GameObject bowlingCamera;
    [SerializeField] private GameObject ballCamera;


    private void Awake()
    {
        BowlerManager.onAimingStarted += EnableAimingCamera;
        BowlerManager.onBowlingStarted += EnableBowlingCamera;
        AiBatsman.onBallHit += EnableBallCamera;

        Ball.onBallHitGround += BallHitGroundCallback;
    }

    private void OnDestroy()
    {
        BowlerManager.onAimingStarted -= EnableAimingCamera;
        BowlerManager.onBowlingStarted -= EnableBowlingCamera;
        AiBatsman.onBallHit -= EnableBallCamera;

        Ball.onBallHitGround -= BallHitGroundCallback;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableAimingCamera()
    {
        aimingCamera.SetActive(true);
        bowlingCamera.SetActive(false);
        ballCamera.SetActive(false);

    }

    private void EnableBowlingCamera()
    {
        bowlingCamera.SetActive(true);
        aimingCamera.SetActive(false);
        ballCamera.SetActive(false);

    }

    private void EnableBallCamera(Transform ball)
    {
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().Follow = ball;
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().LookAt = ball;

        ballCamera.SetActive(true);
        bowlingCamera.SetActive(false);
        aimingCamera.SetActive(false);
        

    }

    private void BallHitGroundCallback(Vector3 hitPosition)
    {
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().Follow = null;
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().LookAt = null;
    }
}
