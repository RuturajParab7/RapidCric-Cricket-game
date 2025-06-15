using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource bowlerShootSound;
    [SerializeField] private AudioSource ballHitGroundSound;
    [SerializeField] private AudioSource wicketSound;
    [SerializeField] private AudioSource batHitBallSound;
   
    void Start()
    {
        PlayerBowler.onBallThrown += PlayBowlerSound;
        PlayerBatsman.onBallHit += PlayBatHitBallSound;

        AiBowler.onBallThrown += PlayBowlerSound;
        AiBatsman.onBallHit += PlayBatHitBallSound;

        Ball.onBallHitGround += PlayBallHitGroundSound;
        Ball.onBallHitStump += PlayWicketSound;
        
    }

    private void OnDestroy()
    {
        PlayerBowler.onBallThrown -= PlayBowlerSound;
        PlayerBatsman.onBallHit -= PlayBatHitBallSound;
        AiBowler.onBallThrown -= PlayBowlerSound;
        AiBatsman.onBallHit -= PlayBatHitBallSound;

        Ball.onBallHitGround -= PlayBallHitGroundSound;
        Ball.onBallHitStump -= PlayWicketSound;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayBowlerSound(float nothingUseful)
    {
        bowlerShootSound.pitch = Random.Range(1.2f , 1.3f);
        bowlerShootSound.Play();
    }

    private void PlayBallHitGroundSound(Vector3 nothingReallyUseful)
    {
        ballHitGroundSound.pitch = Random.Range(.95f, 1.3f);
        ballHitGroundSound.Play();
    }

    private void PlayWicketSound()
    {
        wicketSound.pitch = Random.Range(.95f, 1.3f);
        wicketSound.Play();
    }

    private void PlayBatHitBallSound(Transform importantNahiye)
    {
        batHitBallSound.pitch = Random.Range(.95f, 1.3f);
        batHitBallSound.Play();
    }
}
