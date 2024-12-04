using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wisp : MonoBehaviour, ICollectible
{
    public static event Action OnCoinCollected;
    public float speed = 5.0f;
    public float hoverAmplitude = 0.2f; // Height of the hover motion
    public float hoverSpeed = 8.0f;     // Speed of the hover motion
    private Rigidbody2D rb;

    private bool hasTarget;
    private Vector3 targetPos;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Save the initial position for hovering animation
        startPosition = transform.position;
    }

    public void Collect()
    {
        GameManager.instance.WispCollected();
        Destroy(gameObject);
        OnCoinCollected?.Invoke();
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (targetPos - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * speed;
        }
        else
        {
            Hover();
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPos = position;
        hasTarget = true;
    }

    private void Hover()
    {
        // Create a hover effect using a sine wave
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = startPosition + new Vector3(0, hoverOffset, 0);
    }
}
