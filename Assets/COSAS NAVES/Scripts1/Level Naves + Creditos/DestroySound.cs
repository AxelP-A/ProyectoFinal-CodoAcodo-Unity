using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    // Este script existe, xq al usar timeScale 0, algunos sonidos no pueden destruirse, por ende lo 
    // logramos destruir de esta forma.
    AudioSource audioSource;
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // Cuando deje de reproducir, borra el objeto
        if(!audioSource.isPlaying)
        Destroy(gameObject);
    }
}
