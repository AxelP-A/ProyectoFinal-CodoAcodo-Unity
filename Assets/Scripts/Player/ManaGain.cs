using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGain : MonoBehaviour
{
    public int manaGainAmmount;
    bool triggered;


    void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
        CharacterController2D controller = collision.GetComponent<CharacterController2D>();

        if(!triggered && (controller.mana != controller.manaQuantity)){
            triggered = true;
            // Le damos mana al jugador
            controller.mana += manaGainAmmount;
            Mathf.Clamp(controller.mana, controller.mana, controller.manaQuantity);
            //Actualizamos la visual
            controller.ManaController();

            // Destruimos el objeto
            Destroy(gameObject);
        }       
       }
    }
}
