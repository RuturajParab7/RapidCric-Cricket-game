using UnityEngine;

public class BowlerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerBowler playerBowler;

    private void ThrowBall()
    {
        playerBowler.ThrowBall();
    }

}
