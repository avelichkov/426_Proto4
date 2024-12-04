using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour, ICollectible
{
    public float speed = 5.0f;
    public float hoverAmplitude = 0.2f; // Hover animation amplitude
    public float hoverSpeed = 2.0f;     // Hover animation speed
    public float combineRadius = 2.0f; // Combine radius

    public GameObject threeWispPrefab; // Reference to the new prefab
    private Rigidbody2D rb;

    private bool hasTarget;
    private Vector3 targetPos;
    private Vector3 startPosition;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        startPosition = transform.position;
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckForCombination), 0.5f, 0.5f); // Periodically check for combination
        StartCoroutine(SelfDestructSequence()); // Start self-destruction countdown
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
        GameManager.instance.WispCollected(1); // Add 1 point toward upgrades
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

    private void CheckForCombination()
    {
        Collider2D[] nearbyWisps = Physics2D.OverlapCircleAll(transform.position, combineRadius);
        List<Wisp> wispsToCombine = new List<Wisp>();

        foreach (Collider2D collider in nearbyWisps)
        {
            Wisp wisp = collider.GetComponent<Wisp>();
            if (wisp != null && wisp != this)
            {
                wispsToCombine.Add(wisp);
            }
        }

        if (wispsToCombine.Count >= 2)
        {
            CombineWisps(wispsToCombine);
        }
    }

    private void CombineWisps(List<Wisp> wispsToCombine)
    {
        Vector3 averagePosition = transform.position;
        foreach (Wisp wisp in wispsToCombine)
        {
            averagePosition += wisp.transform.position;
        }
        averagePosition /= (wispsToCombine.Count + 1); // Include this wisp

        Instantiate(threeWispPrefab, averagePosition, Quaternion.identity);

        foreach (Wisp wisp in wispsToCombine)
        {
            Destroy(wisp.gameObject);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator SelfDestructSequence()
    {
        yield return new WaitForSeconds(6f); // Wait 3 seconds before starting visibility toggling

        for (int i = 0; i < 3; i++)
        {
            // Toggle visibility off
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.5f);

            // Toggle visibility on
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }

        // Delete the Wisp after 3 seconds of blinking
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, combineRadius);
    }
}
