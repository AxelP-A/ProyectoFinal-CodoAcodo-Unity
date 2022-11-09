using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject shootSound;
    public GameObject explosionSound;
    public GameObject pickupSound;
    public GameObject gameOverSound;
    public GameObject hitSound;
    public GameObject hoverSound;


    public static VFXController instance = null;
    void Awake()
    {
        //If this script does not exit already, use this current instance
        if (instance == null)
            instance = this;

        //If this script already exit, DESTROY this current instance
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlayShootingNoise(){
        GameObject sonido = Instantiate(shootSound);
        sonido.transform.parent = transform;
    }

    public void PlayExplosionSound(){
        GameObject sonido = Instantiate(explosionSound);  
        sonido.transform.parent = transform;   
    }

    public void PlayPickUpSound(){
        GameObject sonido = Instantiate(pickupSound);  
        sonido.transform.parent = transform;   
    }

    public void PlayGameOverSound(){
        GameObject sonido = Instantiate(gameOverSound);  
        sonido.transform.parent = transform;   
    }
    public void PlayHitSound(){
        GameObject sonido = Instantiate(hitSound);  
        sonido.transform.parent = transform;   
    }
    public void PlayButtonHoverSound(){
        GameObject sonido = Instantiate(hoverSound);  
        sonido.transform.parent = transform;   
    }
}
