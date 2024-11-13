using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{

    public GameObject explosionPrefab;

    public void DeathEffects()
    {
        AudioManager.instance.Play("Explosion");
        GameObject explosion = GameObject.Instantiate(explosionPrefab, GameObject.FindGameObjectWithTag("Canvas").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        explosion.transform.localScale = new Vector3(2, 2, 2);
        explosion.transform.localPosition += new Vector3(20, 20, 0);
    }

}
