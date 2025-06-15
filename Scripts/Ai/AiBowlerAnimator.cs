using UnityEngine;

public class AiBowlerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AiBowler aiBowler;

    private void ThrowBall()
    {
        aiBowler.ThrowBall();
    }
}
