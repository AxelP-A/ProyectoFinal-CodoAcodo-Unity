using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    // Start is called before the first frame update
    float duration;
    void Start()
    {
        duration = gameObject.GetComponent<AudioSource>().clip.length;
        //Debug.Log(duration);
        Destroy(gameObject, duration);
    }
}
