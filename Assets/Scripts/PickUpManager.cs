using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance = null;
    public GameObject shieldPickUp;
    [SerializeField] int CantidadDeEscudo;
    public GameObject botequinPickUp;
    public int dropChance; // De 0 a 100

    [SerializeField] GameObject player;
    Player playerScript;
    GameObject shieldImage;
    public int maxCantEscudos;
    void Awake()
    {
        //If this script does not exit already, use this current instance
        if (instance == null)
            instance = this;

        //If this script already exit, DESTROY this current instance
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start(){
        playerScript = player.GetComponent<Player>();
        shieldImage = player.transform.GetChild(1).gameObject;
    }

    public void GiveShield(){
        if(!shieldImage.activeInHierarchy){
            shieldImage.SetActive(true);
        }
        // Le sumamos escudos al player, pero no dejamos que pase de un valor maximo.
        playerScript.shieldAmmount += CantidadDeEscudo;
        playerScript.shieldAmmount = Mathf.Clamp(playerScript.shieldAmmount, 0, maxCantEscudos);
        // Updateamos el canvas
        playerScript.ShieldDisplayController();
    }

    public void RemoveShield(){
        shieldImage.SetActive(false);
    }

    public void HealPlayer(){
        if(playerScript.life < playerScript.startingHP){
            playerScript.life++;
            playerScript.HeartsController();
        }
    }

    public void SpawnPickUp(Transform where){
        int random = Random.Range(0,101);
        if(random >= (100 - dropChance)){
            if(playerScript.life == playerScript.startingHP){
                // Si se esta full hp, spawnear escudo
                GameObject escudito = Instantiate(shieldPickUp);
                //Debug.Log("Se spawneo escudito.");
                escudito.transform.position = where.position + new Vector3(0, 0.5f, 0);
            } else {
                // Sino curita
                GameObject botequin = Instantiate(botequinPickUp);
                botequin.transform.position = where.position - new Vector3(0, 0.5f, 0);
            }
        }
    }
}
