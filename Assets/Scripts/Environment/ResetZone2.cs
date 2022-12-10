using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetZone2 : MonoBehaviour
{

    public GameObject player;

     void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.transform.position = new Vector2(120, -8);
        }
    }
}
