using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wisp : MonoBehaviour, ICollectible
{
    public static event Action OnCoinCollected;
    public float speed = 5.0f;
    Rigidbody2D rb;

    bool hasTarget;
    Vector3 targetPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Collect()
    {
        GameManager.instance.WispCollected();
        Destroy(gameObject);
        OnCoinCollected?.Invoke();
        //GameManager.instance.WispCollected();
    }
    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (targetPos - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * speed;
        }
    }
    public void SetTarget(Vector3 position)
    {
        targetPos = position;
        hasTarget = true;
    }
}
