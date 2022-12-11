using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<Transform> positions = new List<Transform>();
    public float maxSpawnRateInSeconds = 5f;

    void Start(){
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        // Incrementamos la dificultad cada X segundos.
        InvokeRepeating("IncreaseDifficulty", 0f, 5f);
    }
    void SpawnEnemy(){
        GameObject enemy = Instantiate(EnemyPrefab);
        // Cambiamos la pos del enemigo a una random de los starting points.
        enemy.transform.position = positions[Random.Range(0, positions.Count)].position;
        // Cuando sale el proximo enemigo
        ScheduleNextEnemySpawn();
    }
    void ScheduleNextEnemySpawn(){
        float spawnInSecs;
        spawnInSecs = Random.Range(0.7f, maxSpawnRateInSeconds);
        Invoke("SpawnEnemy", spawnInSecs);
    }
    // Para aumentar la dificultad.
    void IncreaseDifficulty(){
        // Si se llego al max, que no se suba
        if(maxSpawnRateInSeconds == 0.1f){
            CancelInvoke("IncreaseDifficulty");
        }
        // Sino que suba
        if(maxSpawnRateInSeconds > 1f){
            maxSpawnRateInSeconds-= 0.1f;
        }
    }
}
