using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //El Game Manager en este caso esta instanciado 
    // para que los enemigos, puedan usar playerReference para saber donde esta el player al apuntar
    // Y no tener que usar GameObject.Find, que seria poco eficiente con todos los objectos en la jerarquia.

    public static GameManager instance = null;
    public GameObject playerReference;
    // Para las explosiones
    public GameObject explosionPrefab;
    public Color defaultExplosionColor;

    // Para la nafta
    public Image nafta;
    public float velocidadDrenado;
    public float porcentajeIncrementa;
    // Para los power ups
    public GameObject naftaPrefab;

    void Awake()
    {
        //If this script does not exit already, use this current instance
        if (instance == null)
            instance = this;

        //If this script already exit, DESTROY this current instance
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlayExplotion(Vector3 pos, Color colorNuevo){
        
        GameObject explosion = Instantiate(explosionPrefab);
        // We set the transform
        explosion.transform.position = pos;
        // And the color
        if(colorNuevo != defaultExplosionColor)
        {
            explosion.GetComponent<SpriteRenderer>().color = colorNuevo;
        }
    }

    public void DecreaseFuel(){
        if(nafta.fillAmount != 0){
            nafta.fillAmount -= (velocidadDrenado/100) * Time.deltaTime;
        }
    }
    public void IncreaseFuel(){
        // Incrementamos la nafta en un valor que va de 0 a 1.
        Mathf.Clamp(nafta.fillAmount += (porcentajeIncrementa/100), 0f, 1f);
    }

    void Update(){
        DecreaseFuel();
    }

    public void SpawnFuel(Transform whereTo){
        float random = Random.Range(0,11);
        if(random >= 8 || nafta.fillAmount <= 0.15){
            GameObject bidon = Instantiate(naftaPrefab);
            // Movemos el transform.
            bidon.transform.position = whereTo.position;
        }
    }
}