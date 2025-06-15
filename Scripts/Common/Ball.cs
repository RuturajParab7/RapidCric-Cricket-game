using UnityEngine;
using System;
using System.Xml.Serialization;

public class Ball : MonoBehaviour
{
    [Header("Settings")]
    private bool hasHitGround;
    private bool hasBeenHitByBat;

    [Header("Events")]
    public static Action<Vector3> onBallHitGround;
    public static Action onBallMissed;
    public static Action onBallHitStump;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //detect if ball touches the field
        if (collision.collider.tag == "Field")
            FieldCollidedCallback();
        if (collision.collider.tag == "Stump")
            StumpCollidedCallback();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Miss")) // Fixed: Use `other.CompareTag` instead of `collider.Tag`
            MissTriggeredCallback();
    }

    private void FieldCollidedCallback()
    {
        if (!hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround = true;

        onBallHitGround?.Invoke(transform.position);
    }


    private void MissTriggeredCallback()
    {
        if (hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround= true;

        onBallMissed?.Invoke();
    }

    private void StumpCollidedCallback()
    {
        if (hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround= true;

        onBallHitStump?.Invoke();
    }

    public void GetHitByBat(Vector3 velocity)
    {
        hasBeenHitByBat = true;
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
