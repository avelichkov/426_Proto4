using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{

    public GameObject explosionPrefab;


    public GameObject sprite;
    private float stretch = 0.12f;
    private float stretchtarget = 0.03f;


    public void DeathEffects()
    {
        AudioManager.instance.Play("Explosion");
        GameObject explosion = GameObject.Instantiate(explosionPrefab, GameObject.FindGameObjectWithTag("Canvas").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        explosion.transform.localScale = new Vector3(2, 2, 2);
        explosion.transform.localPosition += new Vector3(20, 20, 0);
    }

    public IEnumerator shootSquashStretch(float timeWaited)
    {
        float currentstretch = 0.15f;
        //squash -> 30 frames
        while(currentstretch > stretch)
        {
            currentstretch -= (stretchtarget / 30);
            sprite.transform.localScale = new Vector3(currentstretch, currentstretch, 0.15f);
            yield return new WaitForSecondsRealtime(timeWaited / 30);
        }

        //return
        while(currentstretch < 0.15f)
        {
            currentstretch += (stretchtarget / 30);
            sprite.transform.localScale = new Vector3(currentstretch, currentstretch, 0.15f);
            yield return new WaitForSecondsRealtime(timeWaited / 30);
        }


        //old code, if the new stuff breaks, default to this please!!!

        //sprite.transform.localScale = new Vector3(stretch, stretch, 0.15f);
        //yield return new WaitForSecondsRealtime(timeWaited);
        //sprite.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        
    }


}
