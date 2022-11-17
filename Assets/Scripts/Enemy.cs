using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float speed = 2f;
    // Para disparar
    public GameObject enemyBullet;
    bool activado = false;

    void Start()
    {
        Destroy(gameObject, 6f); // Problemas cacheando la camara en un prefab.
        // No quiero usar Camera.main xq usa muchos recursos, ni tampoco GameobjectFind.
        InvokeRepeating("FireBullet", 1f, 1.5f); // Deja pasar 1s y llama al metodo.
    }

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

    void FireBullet(){
        // Get a Player Reference.
        // Pregunta si existe, sino, devuelve null.
        Transform playerLocation = GameManager.instance.playerReference != null ? GameManager.instance.playerReference.transform : null;
        if(playerLocation != null){
            // Si existe
            GameObject bullet = Instantiate(enemyBullet);
            // Seteamos la posicion
            bullet.transform.position = transform.GetChild(0).position; // El objeto gun.
            // Calculamos la direccion de la bala. // Ese Vector del medio es un offset random x el sprite.
            Vector2 direccion = playerLocation.position - new Vector3(0.3f, 0.3f, 0) - bullet.transform.position;
            // Seteamos la direccion de la bala.
            Vector3 bulletRotation = Vector3.forward - playerLocation.position;
            bullet.GetComponent<EnemyBullet>().SetDirection(direccion);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        // Nos fijamos si choco contra otra nave o una bala enemiga.
        if( col.tag.Equals("PBullet") || col.tag.Equals("Player")){
            if(activado){
                return;
            }
            activado = true; // Estas 2 lineas es para que no triggeree multiples veces.
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if(col.tag.Equals("Player") && !GameManager.instance.CheckPlayerInvulneravility() || col.tag.Equals("PBullet")){
                // Si choco con el player y no es invulnerable o, si choco con la bala
                // Hacemos la explosion
                GameManager.instance.PlayExplotion(transform.position, new Color(255, 255, 255, 255));
                // Reproducimos el sonido
                VFXController.instance.PlayVFX(VFXController.VFXName.EXPLOSION);
                // Roleamos y si hay suerte spawneamos nafta.
                GameManager.instance.SpawnFuel(transform);
                // Roleamos por un power Up
                PickUpManager.instance.SpawnPickUp(transform);
                //Sumamos puntos al player
                GameManager.instance.IncreaseScore(GameManager.instance.pointsPerEnemy);
                // Destruimos al enemigo y la bala.
                Destroy(gameObject); // Destruimos la nave
            } 
            
            if(col.tag.Equals("PBullet")){
                // Si choca con la bala destruimos la nave y la bala.
                Destroy(col.gameObject);
            }
            //Debug.Log("Enemigo destruido");
        }
    }
}
