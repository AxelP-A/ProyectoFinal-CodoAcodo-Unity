using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    public float startingSpeed;
    //public Camera cam;

    void Start()
    {
        startingSpeed = 8;
        speed = startingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Agarramos la posicion de la bala.
        Vector2 pos = transform.position;
        // Calculamos su trayectoria
        pos = new Vector2(pos.x, pos.y + speed * Time.deltaTime);
        // Update a la posicion de la bala.
        transform.position = pos;

        /* La destruimos si se sale de la camara ( cambiar luego x tiempo)
        Vector2 max = cam.ViewportToWorldPoint(new Vector2(1, 1));
        if(transform.position.y > max.y){
            Destroy(gameObject);
        }
        */
    }
}
