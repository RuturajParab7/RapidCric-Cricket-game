using UnityEngine;

public class PlayerBatsmanAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerBatsman playerbatsman;

    public void StartDetectingHits()
    {
        playerbatsman.StartDetectingHits();
    }

    public void StopDetectingHits()
    {
        playerbatsman.StopDetectingHits();
    }
}
