using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoubleJump : MonoBehaviour
{
   CharacterController2D playerScript;
   [SerializeField] GameObject player;
   [SerializeField] GameObject fog;
   [SerializeField] GameObject bootsVisualEffect1;    
   [SerializeField] GameObject bootsVisualEffect2;   
   [SerializeField] GameObject[] limitWalls;      
   /*[SerializeField] GameObject LimitWall2;     
   [SerializeField] GameObject LimitWall3;     
   [SerializeField] GameObject LimitWall4; */

    // Start is called before the first frame update
    void Start()
    {
       playerScript = player.GetComponent<CharacterController2D>();   
       limitWalls = GameObject.FindGameObjectsWithTag("WallsLimit");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
            playerScript.SetUnlockDoubleJump(true);
            FMODAudioPlayer.Instance.setItemState("ItemPicked");
            FMODAudioPlayer.Instance.StopItemSound();
            Destroy(gameObject);
            Destroy(fog);
            Destroy(bootsVisualEffect1);
            Destroy(bootsVisualEffect2);
            foreach(GameObject wall in limitWalls)
            {
                UnityEngine.Rendering.Universal.Light2D light = wall.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                light.intensity = 0.5f;
            }
       }
    }
}
