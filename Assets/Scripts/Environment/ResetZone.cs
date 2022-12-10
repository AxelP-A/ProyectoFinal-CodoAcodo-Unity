using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResetZone : MonoBehaviour
{

    public GameObject player;
    public GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.transform.position = new Vector2(171.5f, -8);
        }
        if (col.CompareTag("Enemy"))
        {
            boss.transform.position = new Vector2(210f, -9);
        }
    }

}
