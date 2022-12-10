using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    CharacterController2D playerScript;
    [SerializeField] GameObject player;



    // Start is called before the first frame update
    void Start()
    {
       playerScript = player.GetComponent<CharacterController2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      /* if(collision.CompareTag("Player"))
       {
            playerScript.ApplyDamage(1,transform.position);
       }
       if(collision.CompareTag("Enemy"))
       {

       }*/
        int dmgValue = 1;
       	Collider2D[] collidersObjects = Physics2D.OverlapCircleAll(transform.position, 1.3f);
		for (int i = 0; i < collidersObjects.Length; i++)
		{
			if (collidersObjects[i].gameObject.tag == "Enemy" && collidersObjects[i].gameObject != gameObject )
			{
				if (transform.localScale.x < 1)
				{
					dmgValue = -dmgValue;
				}
				collidersObjects[i].gameObject.SendMessage("ApplyDamage", dmgValue);
			}
			else if (collidersObjects[i].gameObject.tag == "Player")
			{
				collidersObjects[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(1, transform.position);
			}
		}


    }
}
