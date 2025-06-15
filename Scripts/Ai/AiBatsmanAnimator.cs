using UnityEngine;

public class AiBatsmanAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AiBatsman aibatsman;

    public void StartDetectingHits()
    {
        aibatsman.StartDetectingHits();
    }
}
