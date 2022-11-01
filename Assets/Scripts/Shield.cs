using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    float speed = 2f;
    //El script del pickup del escudo

    void Update()
    {
        // Agarramos su posicion
        Vector2 position = transform.position;
        //Calculamos una nueva
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        // La fijamos
        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag.Equals("Player")){
            // Agregamos el escudo
            PickUpManager.instance.GiveShield();
            // Hacemos un ruidito
            VFXController.instance.PlayPickUpSound();
            // Lo destruimos
            Destroy(gameObject);
            // Coloco esto xq esta tirando problemas de doble trigger.
        }
    }
}
