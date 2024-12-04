using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float cooldown;

    private PlayerEffects playereff;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playereff = GetComponentInParent<PlayerEffects>();
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameManager.stats[0];
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rotating the shooting direction
        transform.parent.rotation = Quaternion.Euler(0, 0, angle);
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                canFire = true;
                timer = 0;
            }

        }
        if (canFire)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            Rigidbody2D rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
            Vector2 knockbackDir = new Vector2(dir.normalized.x,dir.normalized.y);
            //StartCoroutine(MoveBackwards(rb.position - (knockbackDir * 1f),0.3f));
            //playereff.muzzleFlash();
            StartCoroutine(playereff.shootSquashStretch(0.1f));
        }
    }

    public IEnumerator MoveBackwards(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = rb.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate progress (0 to 1) based on elapsed time
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // Interpolate position between start and target
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, progress);

            // Move the Rigidbody
            rb.MovePosition(newPosition);

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final position is exactly the target position
        rb.MovePosition(targetPosition);
    }
}
