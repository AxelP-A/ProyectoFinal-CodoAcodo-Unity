using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMusic : MonoBehaviour
{

    CharacterController2D playerScript;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<CharacterController2D>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
            FMODAudioPlayer.Instance.setGameSection("Cave");
       }
    }
}
