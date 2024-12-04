using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private GameObject player;
    public float speed;
    private Collider2D zombieCol;
    public GameObject WispPrefab;
    public ParticleSystem deathParticleSystemPrefab; // Reference to the particle system directly

    void Awake()
    {
        transform.position = GetPosition();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private Vector2 GetPosition()
    {
        Vector2 playerPos = GameObject.Find("Player").transform.position;
        float angle = Random.Range(0f, 2 * Mathf.PI);
        float radius = 15.0f;
        float xPos = playerPos.x + Mathf.Cos(angle) * radius;
        float yPos = playerPos.y + Mathf.Sin(angle) * radius;
        return new Vector2(xPos, yPos);
    }

    void Update()
    {
        Vector2 dir = player.transform.position;
        dir.Normalize();
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            // Trigger the death effects
            PlayDeathEffects();

            GameManager.instance.ZombieKilled();
            BulletShot bullet = other.gameObject.GetComponent<BulletShot>();
            bullet.TakeDamage();

            // Spawn Wisp
            Instantiate(WispPrefab, transform.position, Quaternion.identity);

            // Destroy the zombie
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver();
        }
    }

    private void PlayDeathEffects()
    {
        if (deathParticleSystemPrefab != null)
        {
            // Instantiate the ParticleSystem at the zombie's position
            ParticleSystem particles = Instantiate(deathParticleSystemPrefab, transform.position, Quaternion.identity);

            // Play the ParticleSystem
            particles.Play();

            // Destroy the ParticleSystem GameObject after its duration
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }
}
