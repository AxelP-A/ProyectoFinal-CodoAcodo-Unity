using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curita : MonoBehaviour
{
    float speed = 2f;
    //El script del pickup del escudo

    // Update is called once per frame
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
            PickUpManager.instance.HealPlayer();
            // Hacemos un ruidito
            VFXController.instance.PlayVFX(VFXController.VFXName.PICKUP);
            // Lo destruimos
            Destroy(gameObject);
        }
    }
}
