using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReticula : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    public void SpawnBombHere(){
        Instantiate(bombPrefab, transform.position,Quaternion.identity);
    }
}
