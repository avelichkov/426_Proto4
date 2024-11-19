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
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playereff = GetComponentInParent<PlayerEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameManager.stats[0];
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rotating the shooting direction
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
            StartCoroutine(playereff.shootSquashStretch(0.1f));
        }
    }
}
