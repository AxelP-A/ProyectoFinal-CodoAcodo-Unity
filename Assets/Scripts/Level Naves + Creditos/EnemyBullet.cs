using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed; // Velocidad
    Vector2 direction; // Direccion que va a tener
    bool isReady; // Bool que indica si ya se fijo la direccion.
    void Awake()
    {
        speed = 5f;
        isReady = false;
    }

    // Seteamos la direccion de la bala
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        isReady = true;
    }

    void Update()
    {
        if(isReady){
            // Conseguimos la pos actual
            Vector2 position = transform.position;
            // Calculamos su nueva posicion.
            position += direction * speed * Time.deltaTime;
            //Aplicamos
            transform.position = position;
        }
        
    }
}
