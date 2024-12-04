using System.Collections;
using UnityEngine;

public class ThreeWisp : MonoBehaviour, ICollectible
{
    public float speed = 5.0f;
    public float hoverAmplitude = 0.2f; // Hover animation amplitude
    public float hoverSpeed = 2.0f;     // Hover animation speed
    private Rigidbody2D rb;

    private bool hasTarget;
    private Vector3 targetPos;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            MoveToTarget();
        }
        else
        {
            Hover();
        }
    }

    public void Collect()
    {
        GameManager.instance.WispCollected(3); // Adds 3 points toward upgrades
        Destroy(gameObject);
    }

    public void SetTarget(Vector3 position)
    {
        targetPos = position;
        hasTarget = true;
    }

    private void MoveToTarget()
    {
        Vector2 targetDirection = (targetPos - transform.position).normalized;
        rb.velocity = targetDirection * speed;
    }

    private void Hover()
    {
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = startPosition + new Vector3(0, hoverOffset, 0);
    }
}
