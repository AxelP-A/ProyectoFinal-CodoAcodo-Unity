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
    [Header("Cosas de Nafta")]
    public Image nafta;
    public GameObject bordeNafta;
    public float velocidadDrenado; // Recomiendo 3
    public float porcentajeIncrementa;
    // Para los power ups
    public GameObject naftaPrefab;

    // Estado del player para que lo vean los enemigos.
    bool isThePlayerInvul = false;
    // Para manejar los menues.
    public MenuAndButtons menuScript;
    bool isTheGameOver;
    public bool gameState{ // Devuelve el valor de isTheGameOver a otros scripts.
        get 
        {
            return isTheGameOver;
        }
    }
    
    // Para el Score
    [Header("Cosas de Score")]
    [SerializeField] ScoreManager scoreScript;
    public int pointsPerEnemy;
    // Para el Boss
    [Header("Cosas del Jefe")]
    [SerializeField] Transform bossSpawnPoint;
    [SerializeField] GameObject bossPrefab;
    GameObject bossReference = null;
    public GameObject bossBar;
    public Image bossHp;
    [SerializeField] GameObject enemySpawner;

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
        if(nafta.fillAmount != 0 && !isTheGameOver){
            nafta.fillAmount -= (velocidadDrenado/100) * Time.deltaTime;
        } else {
            // Si no hay nafta.
            if(isTheGameOver){ // Primero si fija si el juego ya termino para que no se siga llamando esto.
                return;
            }
            TriggerGameOver();
        }
    }
    public void IncreaseFuel(){
        // Incrementamos la nafta en un valor que va de 0 a 1.
        Mathf.Clamp(nafta.fillAmount += (porcentajeIncrementa/100), 0f, 1f);
    }

    void Update(){
        if(bossReference == null){
            DecreaseFuel();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            TriggerPause(); // Si se presiona escape pausamos o despausamos.
        }
    }

    public void SpawnFuel(Transform whereTo){
        float random = Random.Range(0,11);
        if(random >= 8 || nafta.fillAmount <= 0.15){
            GameObject bidon = Instantiate(naftaPrefab);
            // Movemos el transform.
            bidon.transform.position = whereTo.position;
        }
    }

    public bool TogglePlayerInvul()
    {
        isThePlayerInvul = !isThePlayerInvul;
        //Debug.Log("Invul state>" + isThePlayerInvul);
        return isThePlayerInvul;
    }

    public bool CheckPlayerInvulneravility(){
        if(isThePlayerInvul){
            return true;
        }  else {
            return false;
        }
    }

    public void TriggerGameOver(){
        // Se va a encargar de los eventos de gameOver
        if(playerReference != null){
            PlayExplotion(playerReference.transform.position, Color.white);
            VFXController.instance.PlayVFX(VFXController.VFXName.GAME_OVER);
            Destroy(playerReference);
            playerReference = null; // Para que los enemigos no tiren error al disparar, devolvemos esto a null.
        }
        menuScript.ShowGameOverScreen();
        isTheGameOver = true;
    }

    public void TriggerVictory(){
        // Se encarga de los eventos de cuando ganas
        if(playerReference != null){
            VFXController.instance.PlayVictoySound();
            menuScript.ShowVictoryScreen();
            isTheGameOver = true;
        }
    }

    public void TriggerBoss(){
        // Se encarga del cambio de nivel a Boss Lv
        //Spawneamos el jefe
        if(playerReference != null && bossReference == null){
            bossReference = Instantiate(bossPrefab, bossSpawnPoint);
            // Activamos la bossBar y su animacion.
            bossBar.SetActive(true);
            // Apagamos el sistema de fuel
            DisableFuelSystem();
            // Cambiamos a musica de Jefe
            VFXController.instance.SwapToBossMusic();
        }
    }

    public void DisableEnemySpawning(){
        // Apagamos los Spawns de enemigos.
            Destroy(enemySpawner);
    }

    void DisableFuelSystem(){
        nafta.gameObject.SetActive(false);
        bordeNafta.SetActive(false);

    }

    void TriggerPause(){
        if(!isTheGameOver) // Para que no se pueda pausar si se perdio.
        {
            menuScript.TogglePauseScreen();
        }
    }

    public void IncreaseScore(int ammount){
        scoreScript.UpdateScore(ammount);
    }
}
