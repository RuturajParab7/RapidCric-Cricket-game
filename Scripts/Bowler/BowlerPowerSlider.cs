using UnityEngine;
using UnityEngine.UI;
using System;

public class BowlerPowerSlider : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider powerSlider;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    [Header("Events")]
    public static Action<float> onPowerSliderstopped;

    private bool canMove;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            Move();
    }

    public void StartMoving()
    {
        canMove = true;
    }

    public void StopMoving()
    {
        if (!canMove)
            return;

        canMove = false;
        onPowerSliderstopped?.Invoke(powerSlider.value);
    }

    private void Move()
    {
        powerSlider.value = (Mathf.Sin(Time.time * moveSpeed) + 1) / 2;
    }
}
