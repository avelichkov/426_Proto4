using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D bulletCol;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        bulletCol = GetComponent<Collider2D>();
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        Vector3 rot = transform.position - mousePos;
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;
        float angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        AudioManager.instance.Play("Laser Shot");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
