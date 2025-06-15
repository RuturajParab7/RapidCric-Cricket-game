using UnityEngine;

public class BatsmanCamera : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject batsmanCamera;
    
    [SerializeField] private GameObject ballCamera;


    private void Awake()
    {
        BatsmanManager.onAimingStarted += EnableBatsmanCamera;
        PlayerBatsman.onBallHit += EnableBallCamera;

        Ball.onBallHitGround += BallHitGroundCallback;
    }

    private void OnDestroy()
    {
        BatsmanManager.onAimingStarted -= EnableBatsmanCamera;
        PlayerBatsman.onBallHit -= EnableBallCamera;

        Ball.onBallHitGround -= BallHitGroundCallback;
    }

    

    private void EnableBatsmanCamera()
    {
        batsmanCamera.SetActive(true);
        
        ballCamera.SetActive(false);

    }


    private void EnableBallCamera(Transform ball)
    {
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().Follow = ball;
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().LookAt = ball;

        ballCamera.SetActive(true);
        batsmanCamera.SetActive(false);


    }

    private void BallHitGroundCallback(Vector3 hitPosition)
    {
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().Follow = null;
        ballCamera.GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>().LookAt = null;
    }
}
