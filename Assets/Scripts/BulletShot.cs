using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D bulletCol;

    private int _health;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        _health = (int)GameManager.stats[2];
        transform.localScale *= 1 + ((_health - 1) * 0.5f);
        AudioManager.instance.Play("Laser Shot");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        bulletCol = GetComponent<Collider2D>();
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 dir = mousePos - playerPos;
        Vector3 rot = playerPos - mousePos;
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;
        float angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
