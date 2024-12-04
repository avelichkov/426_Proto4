using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour, ICollectible
{
    public float speed = 5.0f;
    public float hoverAmplitude = 0.2f; // Hover animation amplitude
    public float hoverSpeed = 2.0f;     // Hover animation speed
    public float combineRadius = 10.0f; // New radius (doubled from original)

    public GameObject threeWispPrefab; // Reference to the new prefab
    private Rigidbody2D rb;

    private bool hasTarget;
    private Vector3 targetPos;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckForCombination), 0.5f, 0.5f); // Periodically check for combination
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
        // Find nearby wisps
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

        // Combine if there are 2 others nearby
        if (wispsToCombine.Count >= 2)
        {
            CombineWisps(wispsToCombine);
        }
    }

    private void CombineWisps(List<Wisp> wispsToCombine)
{
    // Calculate the average position for the new combined wisp
    Vector3 averagePosition = transform.position;
    foreach (Wisp wisp in wispsToCombine)
    {
        averagePosition += wisp.transform.position;
    }
    averagePosition /= (wispsToCombine.Count + 1); // Include this wisp

    // Spawn the new "ThreeWisp" prefab before destroying anything
    GameObject threeWisp = Instantiate(threeWispPrefab, averagePosition, Quaternion.identity);

    // Ensure this wisp and nearby wisps are destroyed, but NOT the new prefab
    foreach (Wisp wisp in wispsToCombine)
    {
        Destroy(wisp.gameObject);
    }
    Destroy(this.gameObject);
}

    private void OnDrawGizmosSelected()
    {
        // Visualize the combine radius in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, combineRadius);
    }
}
