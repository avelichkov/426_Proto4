using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{

    public GameObject explosionPrefab;


    public GameObject sprite;
    private float stretch = 0.7f;
    private float stretchtarget = 1f;

    public ParticleSystem gunflash;

    /*
    public void DeathEffects()
    {
        AudioManager.instance.Play("Explosion");
        GameObject explosion = GameObject.Instantiate(explosionPrefab, GameObject.FindGameObjectWithTag("Canvas").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        explosion.transform.localScale = new Vector3(2, 2, 2);
        explosion.transform.localPosition += new Vector3(20, 20, 0);
    }
    */

    public IEnumerator shootSquashStretch(float timeWaited)
    {
        float currentstretch = 1f;
        //squash -> 30 frames
        while(currentstretch > stretch)
        {
            currentstretch -= (stretchtarget / 30);
            sprite.transform.localScale = new Vector3(currentstretch, 0.7f /* * currentstretch*/, 0.3f);
            yield return new WaitForSecondsRealtime(timeWaited / 30);
        }

        //return
        while(currentstretch < 1f)
        {
            currentstretch += (stretchtarget / 30);
            sprite.transform.localScale = new Vector3(currentstretch, 0.7f /* * currentstretch*/, 0.3f);
            yield return new WaitForSecondsRealtime(timeWaited / 30);
        }


        //old code, if the new stuff breaks, default to this please!!!

        //sprite.transform.localScale = new Vector3(stretch, stretch, 0.15f);
        //yield return new WaitForSecondsRealtime(timeWaited);
        //sprite.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        
    }

    /*
    public void muzzleFlash()
    {
        //Vector3 shotAngle = 
        gunflash.transform.rotation = GameObject.Find("RotationPoint").transform.rotation;
        gunflash.transform.Rotate(0, 90, 0);
        gunflash.Play();
        
    }
    */
}
